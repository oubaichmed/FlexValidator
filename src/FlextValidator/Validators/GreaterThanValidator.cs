namespace FlexValidator.Validators;

using System;
using System.Reflection;

public class GreaterThanValidator<T, TProperty> : AbstractComparisonValidator<T, TProperty> where TProperty : IComparable<TProperty>, IComparable {

	public override string Name => "GreaterThanValidator";

	public GreaterThanValidator(TProperty value) : base(value) {
	}

	public GreaterThanValidator(Func<T, TProperty> valueToCompareFunc, MemberInfo member, string memberDisplayName)
		: base(valueToCompareFunc, member, memberDisplayName) {
	}

	public GreaterThanValidator(Func<T, (bool HasValue, TProperty Value)> valueToCompareFunc, MemberInfo member, string memberDisplayName)
		: base(valueToCompareFunc, member, memberDisplayName) {
	}

	public override bool IsValid(TProperty value, TProperty valueToCompare) {
		if (valueToCompare == null)
			return false;

		return value.CompareTo(valueToCompare) > 0;
	}

	public override Comparison Comparison => Validators.Comparison.GreaterThan;

	protected override string GetDefaultMessageTemplate(string errorCode) {
		return Localized(errorCode, Name);
	}
}
