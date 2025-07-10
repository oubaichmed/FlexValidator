namespace FlexValidator.Validators;

using System;
using System.Collections.Generic;

/// <summary>
/// Base class for range validation.
/// </summary>
public abstract class RangeValidator<T, TProperty> : PropertyValidator<T, TProperty>, IBetweenValidator {

	readonly IComparer<TProperty> _explicitComparer;

	public RangeValidator(TProperty from, TProperty to, IComparer<TProperty> comparer) {
		To = to;
		From = from;

		_explicitComparer = comparer;

		if (comparer.Compare(to, from) == -1) {
			throw new ArgumentOutOfRangeException(nameof(to), "To should be larger than from.");
		}
	}

	public TProperty From { get; }
	public TProperty To { get; }

	object IBetweenValidator.From => From;
	object IBetweenValidator.To => To;

	protected abstract bool HasError(TProperty value);

	public override bool IsValid(ValidationContext<T> context, TProperty value) {
		// If the value is null then we abort and assume success.
		// This should not be a failure condition - only a NotNull/NotEmpty should cause a null to fail.
		if (value == null) return true;

		if (HasError(value)) {

			context.MessageFormatter
				.AppendArgument("From", From)
				.AppendArgument("To", To);

			return false;
		}
		return true;
	}

	protected int Compare(TProperty a, TProperty b) {
		return _explicitComparer.Compare(a, b);
	}

	protected override string GetDefaultMessageTemplate(string errorCode) {
		return Localized(errorCode, Name);
	}
}

public static class RangeValidatorFactory {
	public static ExclusiveBetweenValidator<T, TProperty> CreateExclusiveBetween<T,TProperty>(TProperty from, TProperty to)
		where TProperty : IComparable<TProperty>, IComparable =>
		new ExclusiveBetweenValidator<T, TProperty>(from, to, ComparableComparer<TProperty>.Instance);

	public static InclusiveBetweenValidator<T, TProperty> CreateInclusiveBetween<T,TProperty>(TProperty from, TProperty to)
		where TProperty : IComparable<TProperty>, IComparable {
		return new InclusiveBetweenValidator<T, TProperty>(from, to, ComparableComparer<TProperty>.Instance);
	}
}
