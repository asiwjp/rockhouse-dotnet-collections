namespace RockHouse.Collections.Sets
{
    /// <summary>
    /// An abstract implementation of a IOrderedSet. 
    /// </summary>
    /// <typeparam name="T">The type of elements.</typeparam>
    public abstract class AbstractOrderedSet<T> : AbstractSet<T>, IOrderedSet<T>
    {
        #region IOrderedSet
        /// <inheritdoc/>
        public abstract T First { get; }

        /// <inheritdoc/>
        public abstract T Last { get; }
        #endregion
    }
}
