namespace FlexValidator.Validators;

public abstract class NoopPropertyValidator<T,TProperty> : PropertyValidator<T,TProperty> {
	public override bool IsValid(ValidationContext<T> context, TProperty value) => true;
}
