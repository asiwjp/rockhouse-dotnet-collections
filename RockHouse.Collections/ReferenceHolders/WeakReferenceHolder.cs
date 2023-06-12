using System;

namespace RockHouse.Collections.ReferenceHolders
{
    internal class WeakReferenceHolder : AbstractReferenceHolder
    {
        private WeakReference<object> _obj;

        public WeakReferenceHolder(object obj) : base(obj)
        {
            _obj = new WeakReference<object>(obj);
        }

        public override void Set(object obj)
        {
            _obj.SetTarget(obj);
            _hashCode = obj.GetHashCode();
        }

        public override bool TryGet(out object obj)
        {
            return _obj.TryGetTarget(out obj);
        }
    }
}
