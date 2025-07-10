namespace FlexValidator.Validators;

using System;

public class PredicateValidator<T,TProperty> : PropertyValidator<T,TProperty>, IPredicateValidator {

	private readonly Func<T, TProperty, ValidationContext<T>, bool> _predicate;

	public override string Name => "PredicateValidator";

	public PredicateValidator(Func<T, TProperty, ValidationContext<T>, bool> predicate) {
		ArgumentNullException.ThrowIfNull(predicate);
		_predicate = predicate;
	}

	public override bool IsValid(ValidationContext<T> context, TProperty value) {
		if (!_predicate(context.InstanceToValidate, value, context)) {
			return false;
		}

		return true;
	}

	protected override string GetDefaultMessageTemplate(string errorCode) {
		return Localized(errorCode, Name);
	}
}

public interface IPredicateValidator : IPropertyValidator { }
