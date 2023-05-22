namespace RockHouse.Collections.Sets
{
    public abstract class AbstractOrderedSet<T> : AbstractSet<T>, IOrderedSet<T>
    {
        #region IOrderedSet
        public abstract T First { get; }

        public abstract T Last { get; }
        #endregion
    }
}
