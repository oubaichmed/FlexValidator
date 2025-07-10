namespace FlexValidator.Validators;

public class NullValidator<T,TProperty> : PropertyValidator<T,TProperty>, INullValidator {

	public override string Name => "NullValidator";

	public override bool IsValid(ValidationContext<T> context, TProperty value) {
		return value == null;
	}

	protected override string GetDefaultMessageTemplate(string errorCode) {
		return Localized(errorCode, Name);
	}
}

public interface INullValidator : IPropertyValidator {
}
