
namespace FlexValidator.Internal;

using System.Collections.Generic;
using System.Linq;

internal class CompositeValidatorSelector : IValidatorSelector {
	private IEnumerable<IValidatorSelector> _selectors;

	public CompositeValidatorSelector(IEnumerable<IValidatorSelector> selectors) {
		_selectors = selectors;
	}

	public bool CanExecute(IValidationRule rule, string propertyPath, IValidationContext context) {
		return _selectors.Any(s => s.CanExecute(rule, propertyPath, context));
	}
}
