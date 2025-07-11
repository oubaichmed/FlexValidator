namespace FlexValidator.Internal;

using System;
using System.Collections.Generic;
using Validators;

/// <summary>
/// Builds a validation rule and constructs a validator.
/// </summary>
/// <typeparam name="T">Type of object being validated</typeparam>
/// <typeparam name="TProperty">Type of property being validated</typeparam>
internal class RuleBuilder<T, TProperty> : IRuleBuilderOptions<T, TProperty>, IRuleBuilderInitial<T, TProperty>, IRuleBuilderInitialCollection<T,TProperty>, IRuleBuilderOptionsConditions<T, TProperty>, IRuleBuilderInternal<T,TProperty> {

	/// <summary>
	/// The rule being created by this RuleBuilder.
	/// </summary>
	public IValidationRuleInternal<T, TProperty> Rule { get; }

	IValidationRule<T, TProperty> IRuleBuilderInternal<T,TProperty>.Rule => Rule;

	/// <summary>
	/// Parent validator
	/// </summary>
	public AbstractValidator<T> ParentValidator { get; }

	/// <summary>
	/// Creates a new instance of the <see cref="RuleBuilder{T,TProperty}">RuleBuilder</see> class.
	/// </summary>
	public RuleBuilder(IValidationRuleInternal<T, TProperty> rule, AbstractValidator<T> parent) {
		Rule = rule;
		ParentValidator = parent;
	}

	public IRuleBuilderOptions<T, TProperty> SetValidator(IPropertyValidator<T, TProperty> validator) {
		if (validator == null) throw new ArgumentNullException(nameof(validator));
		Rule.AddValidator(validator);
		return this;
	}

	public IRuleBuilderOptions<T, TProperty> SetAsyncValidator(IAsyncPropertyValidator<T, TProperty> validator) {
		if (validator == null) throw new ArgumentNullException(nameof(validator));
		// See if the async validator supports synchronous execution too.
		IPropertyValidator<T, TProperty> fallback = validator as IPropertyValidator<T, TProperty>;
		Rule.AddAsyncValidator(validator, fallback);
		return this;
	}

	public IRuleBuilderOptions<T, TProperty> SetValidator(IValidator<TProperty> validator, params string[] ruleSets) {
		ArgumentNullException.ThrowIfNull(validator);
		var adaptor = new ChildValidatorAdaptor<T,TProperty>(validator, validator.GetType()) {
			RuleSets = ruleSets
		};
		// ChildValidatorAdaptor supports both sync and async execution.
		Rule.AddAsyncValidator(adaptor, adaptor);
		return this;
	}

	public IRuleBuilderOptions<T, TProperty> SetValidator<TValidator>(Func<T, TValidator> validatorProvider, params string[] ruleSets) where TValidator : IValidator<TProperty> {
		ArgumentNullException.ThrowIfNull(validatorProvider);
		var adaptor = new ChildValidatorAdaptor<T,TProperty>((context, _) => validatorProvider(context.InstanceToValidate), typeof (TValidator)) {
			RuleSets = ruleSets
		};
		// ChildValidatorAdaptor supports both sync and async execution.
		Rule.AddAsyncValidator(adaptor, adaptor);
		return this;
	}

	public IRuleBuilderOptions<T, TProperty> SetValidator<TValidator>(Func<T, TProperty, TValidator> validatorProvider, params string[] ruleSets) where TValidator : IValidator<TProperty> {
		ArgumentNullException.ThrowIfNull(validatorProvider);
		var adaptor = new ChildValidatorAdaptor<T,TProperty>((context, val) => validatorProvider(context.InstanceToValidate, val), typeof (TValidator)) {
			RuleSets = ruleSets
		};
		// ChildValidatorAdaptor supports both sync and async execution.
		Rule.AddAsyncValidator(adaptor, adaptor);
		return this;
	}

	IRuleBuilderOptions<T, TProperty> IRuleBuilderOptions<T, TProperty>.DependentRules(Action action) {
		DependentRulesInternal(action);
		return this;
	}

	IRuleBuilderOptionsConditions<T, TProperty> IRuleBuilderOptionsConditions<T, TProperty>.DependentRules(Action action) {
		DependentRulesInternal(action);
		return this;
	}

	private void DependentRulesInternal(Action action) {
		var dependencyContainer = new List<IValidationRuleInternal<T>>();
		// Capture any rules added to the parent validator inside this delegate.
		using (ParentValidator.Rules.Capture(dependencyContainer.Add)) {
			action();
		}

		if (Rule.RuleSets != null && Rule.RuleSets.Length > 0) {
			foreach (var dependentRule in dependencyContainer) {
				if (dependentRule.RuleSets == null) {
					dependentRule.RuleSets = Rule.RuleSets;
				}
			}
		}

		Rule.AddDependentRules(dependencyContainer);
	}

	public void AddComponent(RuleComponent<T,TProperty> component) {
		Rule.Components.Add(component);
	}

}
