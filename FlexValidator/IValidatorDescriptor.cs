namespace FlexValidator;

using System.Collections.Generic;
using System.Linq;
using Internal;
using Validators;

/// <summary>
/// Provides metadata about a validator.
/// </summary>
public interface IValidatorDescriptor {

	/// <summary>
	/// All rules defined in the validator.
	/// </summary>
	IEnumerable<IValidationRule> Rules { get; }

	/// <summary>
	/// Gets the name display name for a property.
	/// </summary>
	string GetName(string property);

	/// <summary>
	/// Gets a collection of validators grouped by property.
	/// </summary>
	ILookup<string, (IPropertyValidator Validator, IRuleComponent Options)> GetMembersWithValidators();

	/// <summary>
	/// Gets validators for a particular property.
	/// </summary>
	IEnumerable<(IPropertyValidator Validator, IRuleComponent Options)> GetValidatorsForMember(string name);

	/// <summary>
	/// Gets rules for a property.
	/// </summary>
	IEnumerable<IValidationRule> GetRulesForMember(string name);
}
