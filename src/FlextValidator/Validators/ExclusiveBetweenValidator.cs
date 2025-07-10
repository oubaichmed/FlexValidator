namespace FlexValidator.Validators;

using System.Collections.Generic;

/// <summary>
/// Performs range validation where the property value must be between the two specified values (exclusive).
/// </summary>
public class ExclusiveBetweenValidator<T, TProperty> : RangeValidator<T, TProperty> {

	public override string Name => "ExclusiveBetweenValidator";

	public ExclusiveBetweenValidator(TProperty from, TProperty to, IComparer<TProperty> comparer) : base(from, to, comparer) {
	}

	protected override bool HasError(TProperty value) {
		return Compare(value, From) <= 0 || Compare(value, To) >= 0;
	}
}

public interface IBetweenValidator : IPropertyValidator {
	object From { get; }
	object To { get; }
}
