namespace FlexValidator.Validators;

using System;
using System.Collections;
using System.Collections.Generic;

public class NotEmptyValidator<T,TProperty> : PropertyValidator<T, TProperty>, INotEmptyValidator {

	public override string Name => "NotEmptyValidator";

	public override bool IsValid(ValidationContext<T> context, TProperty value) {
		if (value == null) {
			return false;
		}

		if (value is string s && string.IsNullOrWhiteSpace(s)) {
			return false;
		}

		if (value is ICollection col && col.Count == 0) {
			return false;
		}

		if (value is IEnumerable e && IsEmpty(e)) {
			return false;
		}

		return !EqualityComparer<TProperty>.Default.Equals(value, default);
	}

	protected override string GetDefaultMessageTemplate(string errorCode) {
		return Localized(errorCode, Name);
	}

	private static bool IsEmpty(IEnumerable enumerable) {
		var enumerator = enumerable.GetEnumerator();

		using (enumerator as IDisposable) {
			return !enumerator.MoveNext();
		}
	}
}

public interface INotEmptyValidator : IPropertyValidator {
}
