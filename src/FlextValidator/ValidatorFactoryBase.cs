namespace FlexValidator;

using System;

/// <summary>
/// Factory for creating validators
/// </summary>
[Obsolete("IValidatorFactory and its implementors are deprecated and will be removed in a future release. Please use the Service Provider directly (or a DI container). For details see https://github.com/FlexValidator/FlexValidator/issues/1961")]
public abstract class ValidatorFactoryBase : IValidatorFactory {
	/// <summary>
	/// Gets a validator for a type
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public IValidator<T> GetValidator<T>() {
		return (IValidator<T>)GetValidator(typeof(T));
	}
	/// <summary>
	/// Gets a validator for a type
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public IValidator GetValidator(Type type) {
		var genericType = typeof(IValidator<>).MakeGenericType(type);
		return CreateInstance(genericType);
	}

	/// <summary>
	/// Instantiates the validator
	/// </summary>
	/// <param name="validatorType"></param>
	/// <returns></returns>
	public abstract IValidator CreateInstance(Type validatorType);
}
