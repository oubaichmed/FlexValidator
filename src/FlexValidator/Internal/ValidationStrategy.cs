﻿

namespace FlexValidator.Internal;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public class ValidationStrategy<T>
{
    private List<string> _properties;
    private List<string> _ruleSets;
    private bool _throw = false;
    private IValidatorSelector _customSelector;

    internal ValidationStrategy()
    {
    }

    /// <summary>
    /// Indicates that only the specified properties should be validated.
    /// </summary>
    /// <param name="properties">The property names to validate.</param>
    /// <returns></returns>
    public ValidationStrategy<T> IncludeProperties(params string[] properties)
    {
        if (_properties == null)
        {
            _properties = new List<string>(properties);
        }
        else
        {
            _properties.AddRange(properties);
        }

        return this;
    }

    /// <summary>
    /// Indicates that only the specified properties should be validated.
    /// </summary>
    /// <param name="propertyExpressions">The properties to validate, defined as expressions.</param>
    /// <returns></returns>
    public ValidationStrategy<T> IncludeProperties(params Expression<Func<T, object>>[] propertyExpressions)
    {
        if (_properties == null)
        {
            _properties = new List<string>(MemberNameValidatorSelector.MemberNamesFromExpressions(propertyExpressions));
        }
        else
        {
            _properties.AddRange(MemberNameValidatorSelector.MemberNamesFromExpressions(propertyExpressions));
        }

        return this;
    }

    /// <summary>
    /// Indicates that all rules not in a rule-set should be included for validation (the equivalent of calling IncludeRuleSets("default")).
    /// This method can be combined with IncludeRuleSets.
    /// </summary>
    /// <returns></returns>
    public ValidationStrategy<T> IncludeRulesNotInRuleSet()
    {
        _ruleSets ??= new List<string>();
        _ruleSets.Add(RulesetValidatorSelector.DefaultRuleSetName);
        return this;
    }

    /// <summary>
    /// Indicates that all rules should be executed, regardless of whether or not they're in a ruleset.
    /// This is the equivalent of IncludeRuleSets("*").
    /// </summary>
    /// <returns></returns>
    public ValidationStrategy<T> IncludeAllRuleSets()
    {
        _ruleSets ??= new List<string>();
        _ruleSets.Add(RulesetValidatorSelector.WildcardRuleSetName);
        return this;
    }

    /// <summary>
    /// Indicates that only the specified rule sets should be validated.
    /// </summary>
    /// <param name="ruleSets">The names of the rulesets to validate.</param>
    /// <returns></returns>
    public ValidationStrategy<T> IncludeRuleSets(params string[] ruleSets)
    {
        if (ruleSets != null && ruleSets.Length > 0)
        {
            if (_ruleSets == null)
            {
                _ruleSets = new List<string>(ruleSets);
            }
            else
            {
                _ruleSets.AddRange(ruleSets);
            }
        }

        return this;
    }

    /// <summary>
    /// Indicates that the specified selector should be used to control which rules are executed.
    /// </summary>
    /// <param name="selector">The custom selector to use</param>
    /// <returns></returns>
    public ValidationStrategy<T> UseCustomSelector(IValidatorSelector selector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        _customSelector = selector;
        return this;
    }

    /// <summary>
    /// Indicates that the validator should throw an exception if it fails, rather than return a validation result.
    /// </summary>
    /// <returns></returns>
    public ValidationStrategy<T> ThrowOnFailures()
    {
        _throw = true;
        return this;
    }

    private IValidatorSelector GetSelector()
    {
        IValidatorSelector selector = null;

        if (_properties != null || _ruleSets != null || _customSelector != null)
        {
            var selectors = new List<IValidatorSelector>(3);

            if (_customSelector != null)
            {
                selectors.Add(_customSelector);
            }

            if (_properties != null)
            {
                selectors.Add(ValidatorOptions.Global.ValidatorSelectors.MemberNameValidatorSelectorFactory(_properties.ToArray()));
            }

            if (_ruleSets != null)
            {
                selectors.Add(ValidatorOptions.Global.ValidatorSelectors.RulesetValidatorSelectorFactory(_ruleSets.ToArray()));
            }

            selector = selectors.Count == 1 ? selectors[0] : ValidatorOptions.Global.ValidatorSelectors.CompositeValidatorSelectorFactory(selectors);
        }
        else
        {
            selector = ValidatorOptions.Global.ValidatorSelectors.DefaultValidatorSelectorFactory();
        }

        return selector;
    }

    internal ValidationContext<T> BuildContext(T instance)
    {
        return new ValidationContext<T>(instance, null, GetSelector())
        {
            ThrowOnFailures = _throw
        };
    }
}