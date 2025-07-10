
namespace FlexValidator.Internal;

using System;
using System.Linq.Expressions;
using System.Reflection;

/// <summary>
/// Useful extensions
/// </summary>
public static class Extensions {

	/// <summary>
	/// Gets a MemberInfo from a member expression.
	/// </summary>
	public static MemberInfo GetMember<T, TProperty>(this Expression<Func<T, TProperty>> expression) {
		var memberExp = RemoveUnary(expression.Body) as MemberExpression;

		if (memberExp == null) {
			return null;
		}

		Expression currentExpr = memberExp.Expression;

		// Unwind the expression to get the root object that the expression acts upon.
		while (true) {
			currentExpr = RemoveUnary(currentExpr);

			if (currentExpr != null && currentExpr.NodeType == ExpressionType.MemberAccess) {
				currentExpr = ((MemberExpression)currentExpr).Expression;
			} else {
				break;
			}
		}

		if (currentExpr == null || currentExpr.NodeType != ExpressionType.Parameter) {
			return null; // We don't care if we're not acting upon the model instance.
		}

		return memberExp.Member;
	}

	private static Expression RemoveUnary(Expression toUnwrap) {
		if (toUnwrap is UnaryExpression) {
			return ((UnaryExpression)toUnwrap).Operand;
		}

		return toUnwrap;
	}
}
