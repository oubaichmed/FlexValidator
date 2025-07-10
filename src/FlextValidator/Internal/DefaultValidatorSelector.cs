
namespace FlexValidator.Internal;

using System;
using System.Linq;

/// <summary>
/// Default validator selector that will execute all rules that do not belong to a RuleSet.
/// </summary>
public class DefaultValidatorSelector : IValidatorSelector {
	/// <summary>
	/// Determines whether or not a rule should execute.
	/// </summary>
	/// <param name="rule">The rule</param>
	/// <param name="propertyPath">Property path (eg Customer.Address.Line1)</param>
	/// <param name="context">Contextual information</param>
	/// <returns>Whether or not the validator can execute.</returns>
	public bool CanExecute(IValidationRule rule, string propertyPath, IValidationContext context) {
		// By default we ignore any rules part of a RuleSet.
		if (rule.RuleSets != null && rule.RuleSets.Length > 0 && !rule.RuleSets.Contains(RulesetValidatorSelector.DefaultRuleSetName, StringComparer.OrdinalIgnoreCase)) {
			return false;
		}

		return true;
	}
}
