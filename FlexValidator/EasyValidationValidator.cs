namespace FlexValidator; 
using FlexValidator.Internal;
using FlexValidator.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

public class EasyValidationValidator : ComponentBase
{
    [Inject] private IServiceProvider ServiceProvider { get; set; } = default!;
    [CascadingParameter] private EditContext? CurrentEditContext { get; set; }
    [Parameter] public IValidator? Validator { get; set; }
    [Parameter] public bool DisableAssemblyScanning { get; set; }
    [Parameter] public Action<ValidationStrategy<object>>? Options { get; set; }
    internal Action<ValidationStrategy<object>>? ValidateOptions { get; set; }
    internal Dictionary<FieldIdentifier, List<ValidationFailure>>? LastValidationResult { get; set; }
    public bool Validate(Action<ValidationStrategy<object>>? options = null)
    {
        if (CurrentEditContext is null)
        {
            throw new NullReferenceException(nameof(CurrentEditContext));
        }
        ValidateOptions = options;
        try
        {
            return CurrentEditContext.Validate();
        }
        finally
        {
            ValidateOptions = null;
        }
    }
    /// <summary>
    /// Valide ceci <see cref="EditContext"/>.
    /// </summary>
    /// <returns>True s’il n’y a aucun message de validation après la validation ; sinon faux.</returns>
    public async Task<bool> ValidateAsync(Action<ValidationStrategy<object>>? options = null)
    {
        if (CurrentEditContext is null)
        {
            throw new NullReferenceException(nameof(CurrentEditContext));
        }
        ValidateOptions = options;
        try
        {
            CurrentEditContext.Validate();
            if (!CurrentEditContext!.Properties.TryGetValue(
                    EditContextEasyValidationExtensions.PendingAsyncValidation, out var asyncValidationTask))
            {
                throw new InvalidOperationException("Aucun résultat de validation en attente trouvé");
            }
            await (Task<ValidationResult>)asyncValidationTask;
            return !CurrentEditContext.GetValidationMessages().Any();
        }
        finally
        {
            ValidateOptions = null;
        }
    }
    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException($"{nameof(EasyValidationValidator)} nécessite un paramètre en cascade" +
                $" de type {nameof(EditContext)}.Par exemple, vous pouvez utiliser  {nameof(EasyValidationValidator)} " +
                $"à l'intérieur d'un {nameof(EditForm)}.");
        }
        CurrentEditContext.AddFlexValidator(ServiceProvider, DisableAssemblyScanning, Validator, this);
    }
    /// <summary>
    /// Obtient tous les détails du dernier résultat de validation, éventuellement par champ.
    /// </summary>
    /// <param name="fieldIdentifier">S'il est défini, renvoie uniquement les échecs de validation relatifs au champ donné.</param>
    /// <returns>Échecs de validation.</returns>
    public ValidationFailure[] GetFailuresFromLastValidation(FieldIdentifier? fieldIdentifier = null)
    {
        if (LastValidationResult is null)
            return Array.Empty<ValidationFailure>();
        if (fieldIdentifier is null)
            return LastValidationResult.Values.SelectMany(f => f).ToArray();
        if (!LastValidationResult.TryGetValue(fieldIdentifier.Value, out var failures))
            return Array.Empty<ValidationFailure>();
        return failures.ToArray();
    }
}