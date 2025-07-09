namespace FlexValidator.Validators;

using System;
using System.Reflection;

public class LessThanOrEqualValidator<T, TProperty> : AbstractComparisonValidator<T, TProperty>, ILessThanOrEqualValidator where TProperty : IComparable<TProperty>, IComparable {

	public override string Name => "LessThanOrEqualValidator";

	public LessThanOrEqualValidator(TProperty value) : base(value) {
	}

	public LessThanOrEqualValidator(Func<T, TProperty> valueToCompareFunc, MemberInfo member, string memberDisplayName)
		: base(valueToCompareFunc, member, memberDisplayName) {
	}

	public LessThanOrEqualValidator(Func<T, (bool HasValue, TProperty Value)> valueToCompareFunc, MemberInfo member, string memberDisplayName)
		: base(valueToCompareFunc, member, memberDisplayName) {
	}

	public override bool IsValid(TProperty value, TProperty valueToCompare) {
		if (valueToCompare == null)
			return false;

		return value.CompareTo(valueToCompare) <= 0;
	}

	protected override string GetDefaultMessageTemplate(string errorCode) {
		return Localized(errorCode, Name);
	}

	public override Comparison Comparison => Comparison.LessThanOrEqual;
}

public interface ILessThanOrEqualValidator : IComparisonValidator { }
