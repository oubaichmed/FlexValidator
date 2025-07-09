namespace FlexValidator.Internal;

/// <summary>
/// AbstractValidator implementation for containing child rules.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class ChildRulesContainer<T> : InlineValidator<T> {

	/// <summary>
	/// Used to keep track of rulesets from parent that need to be applied
	/// to child rules in the case of multiple nested child rules.
	/// </summary>
	/// <see cref="DefaultValidatorExtensions.ChildRules{T,TProperty}"/>
	internal string[] RuleSetsToApplyToChildRules;

}
