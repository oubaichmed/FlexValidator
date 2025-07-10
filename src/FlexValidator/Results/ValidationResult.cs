namespace FlexValidator.Results;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The result of running a validator
/// </summary>
[Serializable]
public class ValidationResult
{
    private List<ValidationFailure> _errors;

    /// <summary>
    /// Whether validation succeeded
    /// </summary>
    public virtual bool IsValid => Errors.Count == 0;

    /// <summary>
    /// A collection of errors
    /// </summary>
    public List<ValidationFailure> Errors
    {
        get => _errors;
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // Ensure any nulls are removed and the list is copied
            // to be consistent with the constructor below.
            _errors = value.Where(failure => failure != null).ToList();
        }
    }

    /// <summary>
    /// The RuleSets that were executed during the validation run.
    /// </summary>
    public string[] RuleSetsExecuted { get; set; }

    /// <summary>
    /// Creates a new ValidationResult
    /// </summary>
    public ValidationResult()
    {
        _errors = new List<ValidationFailure>();
    }

    /// <summary>
    /// Creates a new ValidationResult from a collection of failures
    /// </summary>
    /// <param name="failures">Collection of <see cref="ValidationFailure"/> instances which is later available through the <see cref="Errors"/> property.</param>
    /// <remarks>
    /// Any nulls will be excluded.
    /// The list is copied.
    /// </remarks>
    public ValidationResult(IEnumerable<ValidationFailure> failures)
    {
        _errors = failures.Where(failure => failure != null).ToList();
    }

    /// <summary>
    /// Creates a new ValidationResult by combining several other ValidationResults.
    /// </summary>
    /// <param name="otherResults"></param>
    public ValidationResult(IEnumerable<ValidationResult> otherResults)
    {
        _errors = otherResults.SelectMany(x => x.Errors).ToList();
        RuleSetsExecuted = otherResults.Where(x => x.RuleSetsExecuted != null).SelectMany(x => x.RuleSetsExecuted).Distinct().ToArray();
    }

    internal ValidationResult(List<ValidationFailure> errors)
    {
        _errors = errors;
    }

    /// <summary>
    /// Generates a string representation of the error messages separated by new lines.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return ToString(Environment.NewLine);
    }

    /// <summary>
    /// Generates a string representation of the error messages separated by the specified character.
    /// </summary>
    /// <param name="separator">The character to separate the error messages.</param>
    /// <returns></returns>
    public string ToString(string separator)
    {
        return string.Join(separator, _errors.Select(failure => failure.ErrorMessage));
    }

    /// <summary>
    /// Converts the ValidationResult's errors collection into a simple dictionary representation.
    /// </summary>
    /// <returns>A dictionary keyed by property name
    /// where each value is an array of error messages associated with that property.
    /// </returns>
    public IDictionary<string, string[]> ToDictionary()
    {
        return Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );
    }
}