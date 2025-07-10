namespace FlexValidator.Validators;

using System;
using System.Collections.Generic;

internal class ComparableComparer<T> : IComparer<T> where T : IComparable<T> {
	internal static ComparableComparer<T> Instance { get; }

	static ComparableComparer() {
		Instance = new ComparableComparer<T>();
	}

	public int Compare(T x, T y) {
		return x.CompareTo(y);
	}
}
