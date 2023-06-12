namespace RockHouse.Collections
{
    internal abstract class AbstractReferenceHolder
    {
        protected int _hashCode;

        public AbstractReferenceHolder(object obj)
        {
            _hashCode = obj.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (!TryGet(out var obj))
            {
                return false;
            }

            return obj.Equals(other);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public abstract void Set(object obj);

        public abstract bool TryGet(out object obj);
    }
}
