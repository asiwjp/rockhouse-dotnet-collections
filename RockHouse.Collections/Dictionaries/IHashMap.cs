using System.Collections.Generic;

namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// IHashMap is an imitation of the Java language's Map interface.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    public interface IHashMap<K, V> : IDictionary<K, V>, IContainer where K : notnull
    {
        /// <summary>
        /// Stores the value associated with the specified key and returns the old value.
        /// If the key already exists, it will be associated to the new value.
        /// </summary>
        /// <param name="key">Key associated with the value.</param>
        /// <param name="value">Value associated with the specified key.</param>
        /// <returns>The old value associated with the key is returned. If the key is not found, default(V).</returns>
        public V Put(K key, V value);
    }
}
