namespace FlexValidator;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Internal;

internal interface IValidationRuleInternal<T> : IValidationRule<T> {
	ValueTask ValidateAsync(ValidationContext<T> context, CancellationToken cancellation);
	void Validate(ValidationContext<T> context);

	void AddDependentRules(IEnumerable<IValidationRuleInternal<T>> rules);
}

internal interface IValidationRuleInternal<T, TProperty> : IValidationRule<T, TProperty>, IValidationRuleInternal<T> {
	new List<RuleComponent<T,TProperty>> Components { get; }
}
