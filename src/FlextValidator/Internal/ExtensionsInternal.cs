
namespace FlexValidator.Internal;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using Resources;

internal static class ExtensionsInternal {

	// Todo: Replace with ArgumentException.ThrowIfNullOrEmpty once we stop supporting .net 6
	public static void ThrowIfNullOrEmpty([NotNull] string argument, [CallerArgumentExpression("argument")] string paramName = null) {
		if (string.IsNullOrEmpty(argument)) {
			ArgumentNullException.ThrowIfNull(argument, paramName);
			throw new ArgumentException("The value cannot be an empty string", paramName);
		}
	}

	/// <summary>
	/// Checks if the expression is a parameter expression
	/// </summary>
	/// <param name="expression"></param>
	/// <returns></returns>
	internal static bool IsParameterExpression(this LambdaExpression expression) {
		return expression.Body.NodeType == ExpressionType.Parameter;
	}

	/// <summary>
	/// Splits pascal case, so "FooBar" would become "Foo Bar".
	/// </summary>
	/// <remarks>
	/// Pascal case strings with periods delimiting the upper case letters,
	/// such as "Address.Line1", will have the periods removed.
	/// </remarks>
	internal static string SplitPascalCase(this string input) {
		if (string.IsNullOrEmpty(input))
			return input;

		var retVal = new StringBuilder(input.Length + 5);

		for (int i = 0; i < input.Length; ++i) {
			var currentChar = input[i];
			if (char.IsUpper(currentChar)) {
				if ((i > 1 && !char.IsUpper(input[i - 1]))
				    || (i + 1 < input.Length && !char.IsUpper(input[i + 1])))
					retVal.Append(' ');
			}

			if(!char.Equals('.', currentChar)
			   || i + 1 == input.Length
			   || !char.IsUpper(input[i + 1])) {
				retVal.Append(currentChar);
			}
		}

		return retVal.ToString().Trim();
	}

	internal static T GetOrAdd<T>(this IDictionary<string, object> dict, string key, Func<T> value) {
		if (dict.TryGetValue(key, out var tmp)) {
			if (tmp is T result) {
				return result;
			}
		}

		var val = value();
		dict[key] = val;
		return val;
	}

	internal static string ResolveErrorMessageUsingErrorCode(this ILanguageManager languageManager, string errorCode, string fallbackKey) {
		if (errorCode != null) {
			string result = languageManager.GetString(errorCode);

			if (!string.IsNullOrEmpty(result)) {
				return result;
			}
		}
		return languageManager.GetString(fallbackKey);
	}
}
