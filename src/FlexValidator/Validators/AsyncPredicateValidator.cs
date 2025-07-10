namespace FlexValidator.Validators;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Asynchronous custom validator
/// </summary>
public class AsyncPredicateValidator<T,TProperty> : AsyncPropertyValidator<T,TProperty> {
	private readonly Func<T, TProperty, ValidationContext<T>, CancellationToken, Task<bool>> _predicate;

	public override string Name => "AsyncPredicateValidator";

	/// <summary>
	/// Creates a new AsyncPredicateValidator
	/// </summary>
	/// <param name="predicate"></param>
	public AsyncPredicateValidator(Func<T, TProperty, ValidationContext<T>, CancellationToken, Task<bool>> predicate) {
		ArgumentNullException.ThrowIfNull(predicate);
		_predicate = predicate;
	}

	public override Task<bool> IsValidAsync(ValidationContext<T> context, TProperty value, CancellationToken cancellation) {
		return _predicate(context.InstanceToValidate, value, context, cancellation);
	}

	protected override string GetDefaultMessageTemplate(string errorCode) {
		return Localized(errorCode, Name);
	}
}
