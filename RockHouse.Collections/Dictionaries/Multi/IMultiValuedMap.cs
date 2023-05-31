using System.Collections.Generic;

namespace RockHouse.Collections.Dictionaries.Multi
{
    /// <summary>
    /// IHashMap is an imitation of the Java language's Map interface.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    /// <typeparam name="C">The type of the value collection.</typeparam>
    public interface IMultiValuedMap<K, V, C> : IMultiValuedDictionary<K, V, C>
        where K : notnull
        where C : ICollection<V>
    {
        /// <summary>
        /// Gets the collection of values associated with a given key.
        /// Acquired collections maintain an association with the dictionary, and additions and deletions are reflected in the dictionary.
        /// </summary>
        /// <param name="key">Key associated with the value.</param>
        /// <returns>A collection of values associated with a given key. If the key is not found, an empty collection.</returns>
        C Get(K key);

        /// <summary>
        /// Stores the value associated with the specified key.
        /// If the specified key already exists, it is added to the collection of values associated with that key.
        /// </summary>
        /// <param name="key">Key associated with the value.</param>
        /// <param name="value">Value associated with the specified key.</param>
        /// <returns>True if it can be added, false otherwise.</returns>
        bool Put(K key, V value);
    }
}
