namespace FlexValidator.Internal;

using System.Threading;
using System.Threading.Tasks;
using Validators;

internal class RuleComponentForNullableStruct<T, TProperty> : RuleComponent<T, TProperty?> where TProperty : struct {
	private IPropertyValidator<T, TProperty> _propertyValidator;
	private IAsyncPropertyValidator<T, TProperty> _asyncPropertyValidator;

	internal RuleComponentForNullableStruct(IPropertyValidator<T, TProperty> propertyValidator)
		: base(null) {
		_propertyValidator = propertyValidator;
	}

	internal RuleComponentForNullableStruct(IAsyncPropertyValidator<T, TProperty> asyncPropertyValidator, IPropertyValidator<T, TProperty> propertyValidator)
		: base(null, null) {
		_asyncPropertyValidator = asyncPropertyValidator;
	}

	public override IPropertyValidator Validator
		=> (IPropertyValidator) _propertyValidator ?? _asyncPropertyValidator;

	private protected override bool SupportsAsynchronousValidation
		=> _asyncPropertyValidator != null;

	private protected override bool SupportsSynchronousValidation
		=> _propertyValidator != null;

	private protected override bool InvokePropertyValidator(ValidationContext<T> context, TProperty? value) {
		if (!value.HasValue) return true;
		return _propertyValidator.IsValid(context, value.Value);
	}

	private protected override async Task<bool> InvokePropertyValidatorAsync(ValidationContext<T> context, TProperty? value, CancellationToken cancellation) {
		if (!value.HasValue) return true;
		return await _asyncPropertyValidator.IsValidAsync(context, value.Value, cancellation);
	}
}
