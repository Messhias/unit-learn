using System.Collections.Generic;
using Implementations;

namespace Contracts
{
    public interface IImmutableList<T>
    {
        ImmutableList<T> Add(T value);
        ImmutableList<T> Remove(T item);
        ImmutableList<T> Replace(T oldItem, T newItem);
        IEnumerator<T> GetEnumerator();
    }
}