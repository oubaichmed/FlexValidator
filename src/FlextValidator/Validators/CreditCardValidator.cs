namespace FlexValidator.Validators;

using System.Linq;

/// <summary>
/// Ensures that the property value is a valid credit card number.
/// </summary>
public class CreditCardValidator<T> : PropertyValidator<T,string>, ICreditCardValidator {
	// This logic was taken from the CreditCardAttribute in the ASP.NET MVC3 source.

	public override string Name => "CreditCardValidator";

	protected override string GetDefaultMessageTemplate(string errorCode) {
		return Localized(errorCode, Name);
	}

	public override bool IsValid(ValidationContext<T> context, string value) {
		if (value == null) {
			return true;
		}

		value = value.Replace("-", "").Replace(" ", "");

		int checksum = 0;
		bool evenDigit = false;
		// http://www.beachnet.com/~hstiles/cardtype.html
		foreach (char digit in value.ToCharArray().Reverse()) {
			if (!char.IsDigit(digit)) {
				return false;
			}

			int digitValue = (digit - '0') * (evenDigit ? 2 : 1);
			evenDigit = !evenDigit;

			while (digitValue > 0) {
				checksum += digitValue % 10;
				digitValue /= 10;
			}
		}

		return (checksum % 10) == 0;
	}
}

public interface ICreditCardValidator { }
