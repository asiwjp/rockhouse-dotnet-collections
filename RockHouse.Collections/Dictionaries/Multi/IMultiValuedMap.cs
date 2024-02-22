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
        /// Copy all elements into the collection.
        /// The behavior of this method is the same as PutAll.
        /// </summary>
        /// <param name="key">Key associated with the src.</param>
        /// <param name="src">The enumerable from which to copy.</param>
        /// <returns>True if it can be added, false otherwise.</returns>
        bool AddAll(K key, IEnumerable<V> src);

        /// <summary>
        /// Copy all elements into the collection.
        /// The behavior of this method is the same as PutAll.
        /// </summary>
        /// <param name="src">The enumerable from which to copy.</param>
        /// <returns>True if it can be added, false otherwise.</returns>
        bool AddAll(IEnumerable<KeyValuePair<K, V>> src);

        /// <summary>
        /// Removes the values associated with the specified key and returns the removed values.
        /// If key is not not found, nothing is done.
        /// </summary>
        /// <param name="key">Key associated with the value.</param>
        /// <returns>The removed values associated with the key is returned. If the key is not found, an empty collection.</returns>
        public C Delete(K key);

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

        /// <summary>
        /// Copy all elements into the collection.
        /// The behavior of this method is the same as AddAll.
        /// </summary>
        /// <param name="key">Key associated with the src.</param>
        /// <param name="src">The enumerable from which to copy.</param>
        /// <returns>True if it can be added, false otherwise.</returns>
        bool PutAll(K key, IEnumerable<V> src);

        /// <summary>
        /// Copy all elements into the collection.
        /// The behavior of this method is the same as AddAll.
        /// </summary>
        /// <param name="src">The enumerable from which to copy.</param>
        /// <returns>True if it can be added, false otherwise.</returns>
        bool PutAll(IEnumerable<KeyValuePair<K, V>> src);

        /// <summary>
        /// Replace the values associated with the specified key and returns the old values.
        /// If key is not not found, nothing is done.
        /// </summary>
        /// <param name="key">Key associated with the src.</param>
        /// <param name="src">Values associated with the specified key.</param>
        /// <returns>The old values associated with the key is returned. If the key is not found, an empty collection is returned.</returns>
        ICollection<V> Replace(K key, IEnumerable<V> src);
    }
}
