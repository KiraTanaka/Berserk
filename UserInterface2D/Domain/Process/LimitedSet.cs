using System.Collections;
using System.Collections.Generic;

namespace Domain.Process
{
    public class LimitedSet<T> : IEnumerable<T>
    {
        private readonly List<T> _items = new List<T>();
        private readonly int _limit;

        public LimitedSet(int limit)
        {
            _limit = limit;
        }

        public bool IsFull()
        {
            return _items.Count == _limit;
        }

        public int Count => _items.Count;

        public bool Add(T item)
        {
            if (IsFull()) return false;
            if (_items.Contains(item)) return false;
            _items.Add(item);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _items)
                yield return item;
        }
    }
}
