namespace FlexValidator.Validators;

using System;
using System.Collections;
using System.Collections.Generic;

public class EmptyValidator<T,TProperty> : PropertyValidator<T,TProperty> {

	public override string Name => "EmptyValidator";

	public override bool IsValid(ValidationContext<T> context, TProperty value) {
		if (value == null) {
			return true;
		}

		if (value is string s && string.IsNullOrWhiteSpace(s)) {
			return true;
		}

		if (value is ICollection col && col.Count == 0) {
			return true;
		}

		if (value is IEnumerable e && IsEmpty(e)) {
			return true;
		}

		return EqualityComparer<TProperty>.Default.Equals(value, default);
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
