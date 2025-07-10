namespace FlexValidator.Validators;

using System.Threading;
using System.Threading.Tasks;
using Internal;

public abstract class AsyncPropertyValidator<T, TProperty> : IAsyncPropertyValidator<T, TProperty> {
	string IPropertyValidator.GetDefaultMessageTemplate(string errorCode)
		=> GetDefaultMessageTemplate(errorCode);

	/// <inheritdoc />
	public abstract string Name { get; }

	/// <summary>
	/// Returns the default error message template for this validator, when not overridden.
	/// </summary>
	/// <param name="errorCode">The currently configured error code for the validator.</param>
	/// <returns></returns>
	protected virtual string GetDefaultMessageTemplate(string errorCode) => "No default error message has been specified";

	/// <inheritdoc />
	public abstract Task<bool> IsValidAsync(ValidationContext<T> context, TProperty value, CancellationToken cancellation);

	/// <summary>
	/// Retrieves a localized string from the LanguageManager.
	/// If an ErrorCode is defined for this validator, the error code is used as the key.
	/// If no ErrorCode is defined (or the language manager doesn't have a translation for the error code)
	/// then the fallback key is used instead.
	/// </summary>
	/// <param name="errorCode">The currently configured error code for the validator.</param>
	/// <param name="fallbackKey">The fallback key to use for translation, if no ErrorCode is available.</param>
	/// <returns>The translated error message template.</returns>
	protected string Localized(string errorCode, string fallbackKey) {
		return ValidatorOptions.Global.LanguageManager.ResolveErrorMessageUsingErrorCode(errorCode, fallbackKey);
	}
}
