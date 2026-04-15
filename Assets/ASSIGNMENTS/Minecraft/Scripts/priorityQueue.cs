using System;
using System.Collections.Generic;

namespace ASSIGNMENTS.Minecraft.Scripts {
	class PriorityQueue<T> where T : IComparable<T> {
		private readonly List<T> _items = new List<T>();

		public int Count => _items.Count;

		public void Enqueue(T item) {
			_items.Add(item);
			int ci = _items.Count - 1;
			while (ci > 0) {
				int pi = (ci - 1) / 2;
				if (_items[ci].CompareTo(_items[pi]) >= 0) break;
				Swap(ci, pi);
				ci = pi;
			}
		}

		public T Dequeue() {
			if (_items.Count == 0)
				throw new InvalidOperationException("Priority queue is empty.");
			T front = _items[0];
			int last = _items.Count - 1;
			_items[0] = _items[last];
			_items.RemoveAt(last);
			if (_items.Count == 0) return front;
			int pi = 0;
			while (true) {
				int li = pi * 2 + 1;
				if (li >= _items.Count) break;
				int ri = li + 1;
				int si = li;
				if (ri < _items.Count && _items[ri].CompareTo(_items[li]) < 0)
					si = ri;
				if (_items[pi].CompareTo(_items[si]) <= 0) break;
				Swap(pi, si);
				pi = si;
			}
			return front;
		}

		private void Swap(int a, int b) {
			T tmp = _items[a];
			_items[a] = _items[b];
			_items[b] = tmp;
		}
	}
}