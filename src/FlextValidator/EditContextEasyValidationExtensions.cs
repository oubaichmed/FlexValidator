using FlexValidator.Internal;
using FlexValidator.Results;
using FlexValidator;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
 
namespace FlexValidator;
public static class EditContextEasyValidationExtensions
{
    private static readonly char[] Separators = { '.', '[' };
    private static readonly List<string> ScannedAssembly = new();
    private static readonly List<AssemblyScanner.AssemblyScanResult> AssemblyScanResults = new();
    public const string PendingAsyncValidation = "AsyncValidationTask";

    public static void AddFlexValidator(this EditContext editContext, IServiceProvider serviceProvider, bool disableAssemblyScanning, IValidator? validator, EasyValidationValidator FlexValidatorValidator)
    {
        ArgumentNullException.ThrowIfNull(editContext, nameof(editContext));

        var messages = new ValidationMessageStore(editContext);

        editContext.OnValidationRequested +=
            async (sender, _) => await ValidateModel((EditContext)sender!, messages, serviceProvider, disableAssemblyScanning, FlexValidatorValidator, validator);

        editContext.OnFieldChanged +=
            async (_, eventArgs) => await ValidateField(editContext, messages, eventArgs.FieldIdentifier, serviceProvider, disableAssemblyScanning, validator);
    }

    private static async Task ValidateModel(EditContext editContext,
        ValidationMessageStore messages,
        IServiceProvider serviceProvider,
        bool disableAssemblyScanning,
        EasyValidationValidator FlexValidatorValidator,
        IValidator? validator = null)
    {
        validator ??= GetValidatorForModel(serviceProvider, editContext.Model, disableAssemblyScanning);

        if (validator is not null)
        {
            ValidationContext<object> context;

            if (FlexValidatorValidator.ValidateOptions is not null)
            {
                context = ValidationContext<object>.CreateWithOptions(editContext.Model, FlexValidatorValidator.ValidateOptions);
            }
            else if (FlexValidatorValidator.Options is not null)
            {
                context = ValidationContext<object>.CreateWithOptions(editContext.Model, FlexValidatorValidator.Options);
            }
            else
            {
                context = new ValidationContext<object>(editContext.Model);
            }

            var asyncValidationTask = validator.ValidateAsync(context);
            editContext.Properties[PendingAsyncValidation] = asyncValidationTask;
            var validationResults = await asyncValidationTask;

            messages.Clear();
            FlexValidatorValidator.LastValidationResult = new Dictionary<FieldIdentifier, List<ValidationFailure>>();

            foreach (var validationResult in validationResults.Errors)
            {
                var fieldIdentifier = ToFieldIdentifier(editContext, validationResult.PropertyName);
                messages.Add(fieldIdentifier, validationResult.ErrorMessage);

                if (FlexValidatorValidator.LastValidationResult.TryGetValue(fieldIdentifier, out var failures))
                {
                    failures.Add(validationResult);
                }
                else
                {
                    FlexValidatorValidator.LastValidationResult.Add(fieldIdentifier, new List<ValidationFailure> { validationResult });
                }
            }

            editContext.NotifyValidationStateChanged();
        }
    }

    private static async Task ValidateField(EditContext editContext,
        ValidationMessageStore messages,
        FieldIdentifier fieldIdentifier,
        IServiceProvider serviceProvider,
        bool disableAssemblyScanning,
        IValidator? validator = null)
    {
        var properties = new[] { fieldIdentifier.FieldName };
        var context = new ValidationContext<object>(fieldIdentifier.Model, new PropertyChain(), new MemberNameValidatorSelector(properties));

        validator ??= GetValidatorForModel(serviceProvider, fieldIdentifier.Model, disableAssemblyScanning);

        if (validator is not null)
        {
            var validationResults = await validator.ValidateAsync(context);

            messages.Clear(fieldIdentifier);
            messages.Add(fieldIdentifier, validationResults.Errors.Select(error => error.ErrorMessage));

            editContext.NotifyValidationStateChanged();
        }
    }

    private static IValidator? GetValidatorForModel(IServiceProvider serviceProvider, object model, bool disableAssemblyScanning)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(model.GetType());
        try
        {
            if (serviceProvider.GetService(validatorType) is IValidator validator)
            {
                return validator;
            }
        }
        catch (Exception)
        {
            // ignored
        }

        if (disableAssemblyScanning)
        {
            return null;
        }

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(i => i.FullName is not null && !ScannedAssembly.Contains(i.FullName)))
        {
            try
            {
                AssemblyScanResults.AddRange(FlexValidator.AssemblyScanner.FindValidatorsInAssembly(assembly));
            }
            catch (Exception)
            {
                // ignored
            }

            ScannedAssembly.Add(assembly.FullName!);
        }


        var interfaceValidatorType = typeof(IValidator<>).MakeGenericType(model.GetType());
        var modelValidatorType = AssemblyScanResults.FirstOrDefault(i => interfaceValidatorType.IsAssignableFrom(i.InterfaceType))?.ValidatorType;

        if (modelValidatorType is null)
        {
            return null;
        }

        return (IValidator)ActivatorUtilities.CreateInstance(serviceProvider, modelValidatorType);
    }

    private static FieldIdentifier ToFieldIdentifier(in EditContext editContext, in string propertyPath)
    {
        // Cette méthode analyse les chemins de propriété comme 'Prop.Collection[123].ChldProp'
        // et renvoie un FieldIdentifier qui est une paire (instance, propName). Par exemple,
        // cela renverrait la paire (SomeProp.MyCollection[123], "ChildProp"). Il traverse
        // aussi loin que possible dans propertyPath jusqu'à ce qu'il trouve une instance nulle.
        var obj = editContext.Model;
        var nextTokenEnd = propertyPath.IndexOfAny(Separators);

        // Optimisez pour un scénario dans lequel l'analyse n'est pas nécessaire.
        if (nextTokenEnd < 0)
        {
            return new FieldIdentifier(obj, propertyPath);
        }

        ReadOnlySpan<char> propertyPathAsSpan = propertyPath;

        while (true)
        {
            var nextToken = propertyPathAsSpan.Slice(0, nextTokenEnd);
            propertyPathAsSpan = propertyPathAsSpan.Slice(nextTokenEnd + 1);

            object? newObj;
            if (nextToken.EndsWith("]"))
            {
                // C'est un indexeur
                // Ce code suppose les conventions C# (un indexeur nommé Item avec un paramètre)
                nextToken = nextToken.Slice(0, nextToken.Length - 1);
                var prop = obj.GetType().GetProperty("Item");

                if (prop is not null)
                {
                    // we've got an Item property
                    var indexerType = prop.GetIndexParameters()[0].ParameterType;
                    var indexerValue = Convert.ChangeType(nextToken.ToString(), indexerType);

                    newObj = prop.GetValue(obj, new[] { indexerValue });
                }
                else
                {
                    // S'il n'y a pas de propriété Item
                    // Essayez de convertir l'objet en tableau
                    if (obj is object[] array)
                    {
                        var indexerValue = int.Parse(nextToken);
                        newObj = array[indexerValue];
                    }
                    else if (obj is IReadOnlyList<object> readOnlyList)
                    {
                       
                        var indexerValue = int.Parse(nextToken);
                        newObj = readOnlyList[indexerValue];
                    }
                    else
                    {
                        throw new InvalidOperationException($"Could not find indexer on object of type {obj.GetType().FullName}.");
                    }
                }
            }
            else
            {
                var prop = obj.GetType().GetProperty(nextToken.ToString());
                if (prop == null)
                {
                    throw new InvalidOperationException($"Could not find property named {nextToken.ToString()} on object of type {obj.GetType().FullName}.");
                }
                newObj = prop.GetValue(obj);
            }

            if (newObj == null)
            {
                return new FieldIdentifier(obj, nextToken.ToString());
            }
            obj = newObj;
            nextTokenEnd = propertyPathAsSpan.IndexOfAny(Separators);
            if (nextTokenEnd < 0)
            {
                return new FieldIdentifier(obj, propertyPathAsSpan.ToString());
            }
        }
    }
}