using System.Collections;

namespace RockHouse.Collections.ReferenceHolders
{
    internal class HardReferenceHolder : AbstractReferenceHolder
    {
        private object _obj;

        public HardReferenceHolder(object obj, IEqualityComparer comparer) : base(obj, comparer)
        {
            _obj = obj;
        }

        public override void Set(object obj)
        {
            _obj = obj;
            _hashCode = this._comparer.GetHashCode(obj);
        }

        public override bool TryGet(out object obj)
        {
            obj = _obj;
            return true;
        }
    }
}
