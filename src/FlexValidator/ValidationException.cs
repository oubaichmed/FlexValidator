

namespace FlexValidator;

using System;
using System.Collections.Generic;
using Results;
using System.Linq;
using System.Runtime.Serialization;

/// <summary>
/// An exception that represents failed validation
/// </summary>
[Serializable]
public class ValidationException : Exception {
	/// <summary>
	/// Validation errors
	/// </summary>
	public IEnumerable<ValidationFailure> Errors { get; private set; }

	/// <summary>
	/// Creates a new ValidationException
	/// </summary>
	/// <param name="message"></param>
	public ValidationException(string message) : this(message, Enumerable.Empty<ValidationFailure>()) {

	}

	/// <summary>
	/// Creates a new ValidationException
	/// </summary>
	/// <param name="message"></param>
	/// <param name="errors"></param>
	public ValidationException(string message, IEnumerable<ValidationFailure> errors) : base(message) {
		Errors = errors;
	}

	/// <summary>
	/// Creates a new ValidationException
	/// </summary>
	/// <param name="message"></param>
	/// <param name="errors"></param>
	/// <param name="appendDefaultMessage">appends default validation error message to message</param>
	public ValidationException(string message, IEnumerable<ValidationFailure> errors, bool appendDefaultMessage)
		: base(appendDefaultMessage ? $"{message} {BuildErrorMessage(errors)}" : message) {
		Errors = errors;
	}

	/// <summary>
	/// Creates a new ValidationException
	/// </summary>
	/// <param name="errors"></param>
	public ValidationException(IEnumerable<ValidationFailure> errors) : base(BuildErrorMessage(errors)) {
		Errors = errors;
	}

	private static string BuildErrorMessage(IEnumerable<ValidationFailure> errors) {
		var arr = errors.Select(x => $"{Environment.NewLine} -- {x.PropertyName}: {x.ErrorMessage} Severity: {x.Severity.ToString()}");
		return "Validation failed: " + string.Join(string.Empty, arr);
	}

#if NET8_0_OR_GREATER
	[Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
	public ValidationException(SerializationInfo info, StreamingContext context) : base(info, context) {
		Errors = info.GetValue("errors", typeof(IEnumerable<ValidationFailure>)) as IEnumerable<ValidationFailure>;
	}

#if NET8_0_OR_GREATER
	[Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
	public override void GetObjectData(SerializationInfo info, StreamingContext context) {
		if (info == null) throw new ArgumentNullException("info");

		info.AddValue("errors", Errors);
		base.GetObjectData(info, context);
	}
}
