using System;
using System.Collections.Generic;

namespace RockHouse.Collections.Sets
{
    internal class InternalEqualityComparer<T> : IEqualityComparer<ValueTuple<T>>
    {
        public IEqualityComparer<T> EqualityComparer { get; }

        public InternalEqualityComparer(IEqualityComparer<T> comparer)
        {
            this.EqualityComparer = comparer ?? EqualityComparer<T>.Default;
        }

        public bool Equals(ValueTuple<T> x, ValueTuple<T> y)
        {
            return this.EqualityComparer.Equals(x.Item1, y.Item1);
        }

        public int GetHashCode(ValueTuple<T> obj)
        {
            return this.EqualityComparer.GetHashCode(obj.Item1);
        }
    }
}
