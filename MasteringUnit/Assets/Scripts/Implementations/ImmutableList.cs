using System;
using System.Collections;
using System.Collections.Generic;
using Contracts;

namespace Implementations
{
    public sealed class ImmutableList<T> : IEnumerable<T>, IImmutableList<T>
    {
        private readonly T[] _items;
        private IImmutableList<T> _immutableListImplementation;

        public ImmutableList()
        {
            _items = Array.Empty<T>();
        }

        private ImmutableList(T[] items)
        {
            _items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public T this [int index] => _items[index];
        public int Count => _items.Length;

        public ImmutableList<T> Add(T item)
        {
            var newItems = new T[_items.Length + 1];
            Array.Copy(_items, newItems, _items.Length);
            newItems[_items.Length] = item;
            
            return new ImmutableList<T>(newItems);
        }

        public ImmutableList<T> Remove(T item)
        {
            var index = Array.IndexOf(_items, item);
            if (index < 0)
                return this;
            
            var newItems = new T[_items.Length - 1];
            Array.Copy(_items, 0, newItems, 0, index);
            Array.Copy(_items, index + 1, newItems, index, _items.Length - index - 1);
            return new ImmutableList<T>(newItems);
        }

        public ImmutableList<T> Replace(T oldItem, T newItem)
        {
            var index = Array.IndexOf(_items, oldItem);
            if (index < 0)
                return this;

            var newItems = new T[_items.Length];
            Array.Copy(_items, newItems, _items.Length);
            newItems[index] = newItem;
            return new ImmutableList<T>(newItems);
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_items).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}