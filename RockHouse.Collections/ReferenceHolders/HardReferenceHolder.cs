namespace RockHouse.Collections.ReferenceHolders
{
    internal class HardReferenceHolder : AbstractReferenceHolder
    {
        private object _obj;

        public HardReferenceHolder(object obj) : base(obj)
        {
            _obj = obj;
        }

        public override void Set(object obj)
        {
            _obj = obj;
            _hashCode = obj.GetHashCode();
        }

        public override bool TryGet(out object obj)
        {
            obj = _obj;
            return true;
        }
    }
}
