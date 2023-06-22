using System;
using System.Collections;

namespace RockHouse.Collections.ReferenceHolders
{
    internal class WeakReferenceHolder : AbstractReferenceHolder
    {
        private WeakReference<object> _obj;

        public WeakReferenceHolder(object obj, IEqualityComparer? comparer) : base(obj, comparer)
        {
            _obj = new WeakReference<object>(obj);
        }

        public override void Set(object obj)
        {
            _obj.SetTarget(obj);
            _hashCode = _comparer.GetHashCode(obj);
        }

        public override bool TryGet(out object obj)
        {
            return _obj.TryGetTarget(out obj);
        }
    }
}
