namespace FlexValidator.Validators;

public class NotNullValidator<T,TProperty> : PropertyValidator<T,TProperty>, INotNullValidator {

	public override string Name => "NotNullValidator";

	public override bool IsValid(ValidationContext<T> context, TProperty value) {
		return value != null;
	}

	protected override string GetDefaultMessageTemplate(string errorCode) {
		return Localized(errorCode, Name);
	}
}

public interface INotNullValidator : IPropertyValidator {
}
