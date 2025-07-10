namespace FlexValidator.Validators;

using System.Collections.Generic;

/// <summary>
/// Performs range validation where the property value must be between the two specified values (inclusive).
/// </summary>
public class InclusiveBetweenValidator<T, TProperty> : RangeValidator<T, TProperty>, IInclusiveBetweenValidator {

	public override string Name => "InclusiveBetweenValidator";

	public InclusiveBetweenValidator(TProperty from, TProperty to, IComparer<TProperty> comparer) : base(from, to, comparer) {
	}

	protected override bool HasError(TProperty value) {
		return Compare(value, From) < 0 || Compare(value, To) > 0;
	}
}


public interface IInclusiveBetweenValidator : IBetweenValidator { }
