

namespace FlexValidator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Internal;
using Validators;

/// <summary>
/// Used for providing metadata about a validator.
/// </summary>
public class ValidatorDescriptor<T> : IValidatorDescriptor {

	/// <summary>
	/// Rules associated with the validator
	/// </summary>
	public IEnumerable<IValidationRule> Rules { get; }

	/// <summary>
	/// Creates a ValidatorDescriptor
	/// </summary>
	/// <param name="rules"></param>
	public ValidatorDescriptor(IEnumerable<IValidationRule> rules) {
		Rules = rules;
	}

	/// <summary>
	/// Gets the display name or a property property
	/// </summary>
	/// <param name="property"></param>
	/// <returns></returns>
	public virtual string GetName(string property) {
		var nameUsed = Rules
			.Where(x => x.PropertyName == property)
			.Select(x => x.GetDisplayName(null))
			.FirstOrDefault();

		return nameUsed;
	}
	/// <summary>
	/// Gets all members with their associated validators
	/// </summary>
	/// <returns></returns>
	public virtual ILookup<string, (IPropertyValidator Validator, IRuleComponent Options)> GetMembersWithValidators() {
		var query = from rule in Rules
			from component in rule.Components
			select new { propertyName = rule.PropertyName, component };

		return query.ToLookup(x => x.propertyName, x => (x.component.Validator, x.component));
	}

	/// <summary>
	/// Gets validators for a specific member
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public IEnumerable<(IPropertyValidator Validator, IRuleComponent Options)> GetValidatorsForMember(string name) {
		return GetMembersWithValidators()[name];
	}

	/// <summary>
	/// Gets rules for a specific member
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public IEnumerable<IValidationRule> GetRulesForMember(string name) {
		var query = from rule in Rules
			where rule.PropertyName == name
			select rule;

		return query.ToList();
	}

	/// <summary>
	/// Gets the member name from an expression
	/// </summary>
	/// <param name="propertyExpression"></param>
	/// <returns></returns>
	public virtual string GetName(Expression<Func<T, object>> propertyExpression) {
		var member = propertyExpression.GetMember();

		if (member == null) {
			throw new ArgumentException($"Cannot retrieve name as expression '{propertyExpression}' as it does not specify a property.");
		}

		return GetName(member.Name);
	}

	/// <summary>
	/// Gets rules grouped by ruleset
	/// </summary>
	/// <returns></returns>
	public IEnumerable<RulesetMetadata> GetRulesByRuleset() {
		var query = from rule in Rules
			from ruleset in rule.RuleSets
			group rule by ruleset
			into grp
			select new RulesetMetadata(grp.Key, grp);

		return query.ToList();
	}

	/// <summary>
	/// Information about rulesets
	/// </summary>
	public class RulesetMetadata {

		/// <summary>
		/// Creates a new RulesetMetadata
		/// </summary>
		/// <param name="name"></param>
		/// <param name="rules"></param>
		public RulesetMetadata(string name, IEnumerable<IValidationRule> rules) {
			Name = name;
			Rules = rules;
		}

		/// <summary>
		/// Ruleset name
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Rules in the ruleset
		/// </summary>
		public IEnumerable<IValidationRule> Rules { get; }
	}
}
