
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace FlexValidator
{
    public partial class EasyValidationMessage<TValue> : ComponentBase, IDisposable
    {
        private EditContext? _previousEditContext;
        private Expression<Func<TValue>>? _previousFieldAccessor;
        private readonly EventHandler<ValidationStateChangedEventArgs>? _validationStateChangedHandler;
        private FieldIdentifier _fieldIdentifier;
        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
        [CascadingParameter] EditContext CurrentEditContext { get; set; } = default!;
        [Parameter] public Expression<Func<TValue>>? For { get; set; }
        [Parameter] public messageFormat MessageFormat { get; set; } = messageFormat.text;

        [Parameter]
        public int Time { get; set; } = 5;

        public async void StartTimerAsync()
        {
            Time = 5;
            while (Time > 0)
            {
                Time--;
                StateHasChanged();
                await Task.Delay(1000);
            }
        }
        protected override void OnInitialized() => StartTimerAsync();


        public EasyValidationMessage()
        {
            _validationStateChangedHandler = (sender, eventArgs) => StateHasChanged();
        }
        protected override void OnParametersSet()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException($"{GetType()} requires a cascading parameter " +
                    $"of type {nameof(EditContext)}. For example, you can use {GetType()} inside " +
                    $"an {nameof(EditForm)}.");
            }

            if (For == null) // Pas possible sauf si vous spécifiez manuellement T
            {
                throw new InvalidOperationException($"{GetType()} requires a value for the " +
                    $"{nameof(For)} parameter.");
            }
            else if (For != _previousFieldAccessor)
            {
                _fieldIdentifier = FieldIdentifier.Create(For);
                _previousFieldAccessor = For;
            }
            if (CurrentEditContext != _previousEditContext)
            {
                DetachValidationStateChangedListener();
                CurrentEditContext.OnValidationStateChanged += _validationStateChangedHandler;
                _previousEditContext = CurrentEditContext;
            }
            StartTimerAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
        }
        void IDisposable.Dispose()
        {
            DetachValidationStateChangedListener();
            Dispose(disposing: true);
        }
        private void DetachValidationStateChangedListener()
        {
            if (_previousEditContext != null)
            {
                _previousEditContext.OnValidationStateChanged -= _validationStateChangedHandler;
            }
        }
    }
}