namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// An interface representing an ordered dictionary.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    public interface IOrderedDictionary<K, V> : IHashMap<K, V> where K : notnull
    {
        /// <summary>
        /// The first key.
        /// </summary>
        K FirstKey { get; }

        /// <summary>
        /// The last key.
        /// </summary>
        K LastKey { get; }
    }
}
