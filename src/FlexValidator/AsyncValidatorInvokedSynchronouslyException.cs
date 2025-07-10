namespace FlexValidator;

using System;

/// <summary>
/// This exception is thrown when an asynchronous validator is executed synchronously.
/// </summary>
public class AsyncValidatorInvokedSynchronouslyException : InvalidOperationException {
	public Type ValidatorType { get; }

	internal AsyncValidatorInvokedSynchronouslyException() {
	}

	internal AsyncValidatorInvokedSynchronouslyException(Type validatorType, bool wasInvokedByAspNet)
		: base(BuildMessage(validatorType, wasInvokedByAspNet)) {
		ValidatorType = validatorType;
	}

	internal AsyncValidatorInvokedSynchronouslyException(string message) : base(message) {
	}

	private static string BuildMessage(Type validatorType, bool wasInvokedByMvc) {
		if (wasInvokedByMvc) {
			return $"Validator \"{validatorType.Name}\" can't be used with ASP.NET automatic validation as it contains asynchronous rules. ASP.NET's validation pipeline is not asynchronous and can't invoke asynchronous rules. Remove the asynchronous rules in order for this validator to run.";
		}

		return $"Validator \"{validatorType.Name}\" contains asynchronous rules but was invoked synchronously. Please call ValidateAsync rather than Validate.";
	}
}
