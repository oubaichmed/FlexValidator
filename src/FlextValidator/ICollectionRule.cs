namespace FlexValidator;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Represents a rule defined against a collection with RuleForEach.
/// </summary>
/// <typeparam name="T">Root object</typeparam>
/// <typeparam name="TElement">Type of each element in the collection</typeparam>
public interface ICollectionRule<T, TElement> : IValidationRule<T, TElement> {
	/// <summary>
	/// Filter that should include/exclude items in the collection.
	/// </summary>
	public Func<TElement, bool> Filter { get; set; }

	/// <summary>
	/// Asynchronous filter that should include/exclude items in the collection.
	/// </summary>
	public Func<TElement, Task<bool>> AsyncFilter { get; set; }

	/// <summary>
	/// Constructs the indexer in the property name associated with the error message.
	/// By default this is "[" + index + "]"
	/// </summary>
	public Func<T, IEnumerable<TElement>, TElement, int, string> IndexBuilder { get; set; }
}
