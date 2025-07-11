namespace FlexValidator.Validators;

using System;
using System.Text.RegularExpressions;

/// <summary>
/// Defines which mode should be used for email validation.
/// </summary>
public enum EmailValidationMode {
	/// <summary>
	/// Uses a regular expression for email validation. This is the same regex used by the EmailAddressAttribute in .NET 4.x.
	/// </summary>
	[Obsolete("Regex-based email validation is not recommended and is no longer supported.")]
	Net4xRegex,

	/// <summary>
	/// Uses the simplified ASP.NET Core logic for checking an email address, which just checks for the presence of an @ sign.
	/// </summary>
	AspNetCoreCompatible,
}

//Email regex matches the one used in the DataAnnotations EmailAddressAttribute for consistency/parity with DataAnnotations. This is not a fully comprehensive solution, but is "good enough" for most cases.
[Obsolete("Regex-based email validation is not recommended and is no longer supported.")]
public partial class EmailValidator<T> : PropertyValidator<T,string>, IRegularExpressionValidator, IEmailValidator {

	const string _expression = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";

	[GeneratedRegex(_expression, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture, 2000)]
	private static partial Regex LegacyEmailRegex();

	public override string Name => "EmailValidator";

	public override bool IsValid(ValidationContext<T> context, string value) {
		if (value == null) return true;

		if (!LegacyEmailRegex().IsMatch(value)) {
			return false;
		}

		return true;
	}

	public string Expression => _expression;

	protected override string GetDefaultMessageTemplate(string errorCode) {
		return Localized(errorCode, Name);
	}
}

public class AspNetCoreCompatibleEmailValidator<T> : PropertyValidator<T,string>, IEmailValidator {

	public override string Name => "EmailValidator";

	public override bool IsValid(ValidationContext<T> context, string value) {
		if (value == null) {
			return true;
		}

		// only return true if there is only 1 '@' character
		// and it is neither the first nor the last character
		int index = value.IndexOf('@');

		return
			index > 0 &&
			index != value.Length - 1 &&
			index == value.LastIndexOf('@');
	}

	protected override string GetDefaultMessageTemplate(string errorCode) {
		return Localized(errorCode, Name);
	}
}

public interface IEmailValidator { }
