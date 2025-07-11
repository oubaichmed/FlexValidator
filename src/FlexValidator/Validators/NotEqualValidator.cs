namespace FlexValidator.Validators;

using System;
using System.Collections.Generic;
using System.Reflection;

public class NotEqualValidator<T,TProperty> : PropertyValidator<T,TProperty>, IComparisonValidator {
	private readonly IEqualityComparer<TProperty> _comparer;
	private readonly Func<T, TProperty> _func;
	private readonly string _memberDisplayName;

	public override string Name => "NotEqualValidator";

	public NotEqualValidator(Func<T, TProperty> func, MemberInfo memberToCompare, string memberDisplayName, IEqualityComparer<TProperty> equalityComparer = null) {
		_func = func;
		_comparer = equalityComparer;
		_memberDisplayName = memberDisplayName;
		MemberToCompare = memberToCompare;
	}

	public NotEqualValidator(TProperty comparisonValue, IEqualityComparer<TProperty> equalityComparer = null) {
		ValueToCompare = comparisonValue;
		_comparer = equalityComparer;
	}

	public override bool IsValid(ValidationContext<T> context, TProperty value) {
		var comparisonValue = GetComparisonValue(context);
		bool success = !Compare(comparisonValue, value);

		if (!success) {
			context.MessageFormatter.AppendArgument("ComparisonValue", comparisonValue);
			context.MessageFormatter.AppendArgument("ComparisonProperty", _memberDisplayName ?? "");
			return false;
		}

		return true;
	}

	private TProperty GetComparisonValue(ValidationContext<T> context) {
		if (_func != null) {
			return _func(context.InstanceToValidate);
		}

		return ValueToCompare;
	}

	public Comparison Comparison => Comparison.NotEqual;

	public MemberInfo MemberToCompare { get; }
	public TProperty ValueToCompare { get; }

	object IComparisonValidator.ValueToCompare => ValueToCompare;

	protected bool Compare(TProperty comparisonValue, TProperty propertyValue) {
		if(_comparer != null) {
			return _comparer.Equals(comparisonValue, propertyValue);
		}

		return Equals(comparisonValue, propertyValue);
	}

	protected override string GetDefaultMessageTemplate(string errorCode) {
		return Localized(errorCode, Name);
	}
}
