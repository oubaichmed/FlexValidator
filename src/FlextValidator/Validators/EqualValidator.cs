namespace FlexValidator.Validators;

using System;
using System.Collections.Generic;
using System.Reflection;

public class EqualValidator<T,TProperty> : PropertyValidator<T, TProperty>, IEqualValidator {
	readonly Func<T, TProperty> _func;
	private readonly string _memberDisplayName;
	readonly IEqualityComparer<TProperty> _comparer;

	public override string Name => "EqualValidator";


	public EqualValidator(TProperty valueToCompare, IEqualityComparer<TProperty> comparer = null) {
		ValueToCompare = valueToCompare;
		_comparer = comparer;
	}

	public EqualValidator(Func<T, TProperty> comparisonProperty, MemberInfo member, string memberDisplayName, IEqualityComparer<TProperty> comparer = null) {
		_func = comparisonProperty;
		_memberDisplayName = memberDisplayName;
		MemberToCompare = member;
		_comparer = comparer;
	}

	public override bool IsValid(ValidationContext<T> context, TProperty value) {
		var comparisonValue = GetComparisonValue(context);
		bool success = Compare(comparisonValue, value);

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

	public Comparison Comparison => Comparison.Equal;

	public MemberInfo MemberToCompare { get; }
	public TProperty ValueToCompare { get; }

	object IComparisonValidator.ValueToCompare => ValueToCompare;

	protected bool Compare(TProperty comparisonValue, TProperty propertyValue) {
		if (_comparer != null) {
			return _comparer.Equals(comparisonValue, propertyValue);
		}

		return Equals(comparisonValue, propertyValue);
	}

	protected override string GetDefaultMessageTemplate(string errorCode) {
		return Localized(errorCode, Name);
	}
}

public interface IEqualValidator : IComparisonValidator { }
