using System.Collections;
using System.Collections.Generic;

namespace RockHouse.Collections
{
    internal abstract class AbstractReferenceHolder
    {
        protected int _hashCode;
        protected IEqualityComparer _comparer;

        public AbstractReferenceHolder(object obj, IEqualityComparer? comparer)
        {
            _comparer = comparer ?? EqualityComparer<object>.Default;
            _hashCode = _comparer.GetHashCode(obj);
        }

        public override bool Equals(object other)
        {
            if (!TryGet(out var obj))
            {
                return false;
            }

            return _comparer.Equals(obj, other);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public abstract void Set(object obj);

        public abstract bool TryGet(out object obj);
    }
}
