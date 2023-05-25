namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// An abstract implementation of a IOrderedDictionary. 
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    public abstract class AbstractOrderedDictionary<K, V> : AbstractDictionary<K, V>, IOrderedDictionary<K, V> where K : notnull
    {
        #region IOrderedDictionary
        /// <inheritdoc/>
        public abstract K FirstKey { get; }

        /// <inheritdoc/>
        public abstract K LastKey { get; }
        #endregion
    }
}
