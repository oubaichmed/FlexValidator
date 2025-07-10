
namespace FlexValidator;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Defines a validator for a particular type.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IValidator<in T> : IValidator
{
    /// <summary>
    /// Validates the specified instance.
    /// </summary>
    /// <param name="instance">The instance to validate</param>
    /// <returns>A ValidationResult object containing any validation failures.</returns>
    ValidationResult Validate(T instance);

    /// <summary>
    /// Validate the specified instance asynchronously
    /// </summary>
    /// <param name="instance">The instance to validate</param>
    /// <param name="cancellation"></param>
    /// <returns>A ValidationResult object containing any validation failures.</returns>
    Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellation = default);
}

/// <summary>
/// Defines a validator for a particular type.
/// </summary>
public interface IValidator
{
    /// <summary>
    /// Validates the specified instance.
    /// </summary>
    /// <param name="context">A ValidationContext</param>
    /// <returns>A ValidationResult object contains any validation failures.</returns>
    ValidationResult Validate(IValidationContext context);

    /// <summary>
    /// Validates the specified instance asynchronously.
    /// </summary>
    /// <param name="context">A ValidationContext</param>
    /// <param name="cancellation">Cancellation token</param>
    /// <returns>A ValidationResult object contains any validation failures.</returns>
    Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = default);

    /// <summary>
    /// Creates a hook to access various meta data properties
    /// </summary>
    /// <returns>A IValidatorDescriptor object which contains methods to access metadata</returns>
    IValidatorDescriptor CreateDescriptor();

    /// <summary>
    /// Checks to see whether the validator can validate objects of the specified type
    /// </summary>
    bool CanValidateInstancesOfType(Type type);
}
