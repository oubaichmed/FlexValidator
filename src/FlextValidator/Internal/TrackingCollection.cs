namespace FlexValidator.Internal;

using System;
using System.Collections;
using System.Collections.Generic;

internal class TrackingCollection<T> : IEnumerable<T> {
	readonly List<T> _innerCollection = new();
	public event Action<T> ItemAdded;
	private Action<T> _capture = null;

	public void Add(T item) {
		if (_capture == null) {
			_innerCollection.Add(item);
		}
		else {
			_capture(item);
		}

		ItemAdded?.Invoke(item);
	}

	public int Count => _innerCollection.Count;

	public T this[int index] => _innerCollection[index];

	public void Remove(T item) {
		_innerCollection.Remove(item);
	}

	public IDisposable OnItemAdded(Action<T> onItemAdded) {
		ItemAdded += onItemAdded;
		return new EventDisposable(this, onItemAdded);
	}

	internal IDisposable Capture(Action<T> onItemAdded) {
		return new CaptureDisposable(this, onItemAdded);
	}

	public void AddRange(IEnumerable<T> collection) {
		_innerCollection.AddRange(collection);
	}

	public IEnumerator<T> GetEnumerator() {
		return _innerCollection.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

	class EventDisposable : IDisposable {
		readonly TrackingCollection<T> parent;
		readonly Action<T> handler;

		public EventDisposable(TrackingCollection<T> parent, Action<T> handler) {
			this.parent = parent;
			this.handler = handler;
		}

		public void Dispose() {
			parent.ItemAdded -= handler;
		}
	}

	private class CaptureDisposable : IDisposable {
		readonly TrackingCollection<T> _parent;
		readonly Action<T> _old;

		public CaptureDisposable(TrackingCollection<T> parent, Action<T> handler) {
			this._parent = parent;
			this._old = parent._capture;
			parent._capture = handler;
		}

		public void Dispose() {
			_parent._capture = _old;
		}
	}
}
