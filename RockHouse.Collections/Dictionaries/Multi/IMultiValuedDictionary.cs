using System.Collections;
using System.Collections.Generic;

namespace RockHouse.Collections.Dictionaries.Multi
{
    /// <summary>
    /// IMultiValuedDictionary is a dictionary that allows multiple values to be associated with a single key.
    /// No KeyNotFoundException is thrown in this class. If an attempt is made to retrieve a value using an unregistered key, an empty collection will be returned.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    /// <typeparam name="C">The type of the value collection.</typeparam>
    public interface IMultiValuedDictionary<K, V, C> : IContainer, ICollection<KeyValuePair<K, V>>, IEnumerable<KeyValuePair<K, V>>, IEnumerable
        where K : notnull
        where C : System.Collections.Generic.ICollection<V>
    {
        /// <summary>
        /// Accesses the collection of values associated with a given key.
        /// Acquired collections maintain an association with the dictionary, and additions and deletions are reflected in the dictionary.
        /// </summary>
        /// <param name="key">A key associated with a collection of values.</param>
        /// <returns>A collection of values associated with a given key. If the key is not found, an empty collection.</returns>
        C this[K key] { get; set; }

        /// <summary>
        /// Retrieve registered keys.
        /// </summary>
        ICollection<K> Keys { get; }

        /// <summary>
        /// Retrieve all registered values.
        /// </summary>
        ICollection<V> Values { get; }

        /// <summary>
        /// Adds the value associated with the specified key.
        /// If the specified key already exists, it is added to the collection of values associated with that key.
        /// </summary>
        /// <param name="key">Key associated with the value.</param>
        /// <param name="value">Value associated with the specified key.</param>
        /// <returns>True if it can be added, false otherwise.</returns>
        bool Add(K key, V value);

        /// <summary>
        /// Determines whether or not the specified key/value association exists.
        /// </summary>
        /// <param name="key">Key to verify.</param>
        /// <returns>True if a value is associated with the specified key, false otherwise.</returns>
        bool ContainsKey(K key);

        /// <summary>
        /// Gets the collection of values associated with a given key.
        /// Acquired collections maintain an association with the dictionary, and additions and deletions are reflected in the dictionary.
        /// </summary>
        /// <param name="key">Key associated with the value.</param>
        /// <param name="values">A collection of values associated with a given key. If the key is not found, an empty collection.</param>
        /// <returns>True if a value is associated with the specified key, false otherwise.</returns>
        bool TryGetValue(K key, out C values);

        /// <summary>
        /// Removes all value associated with the specified key.
        /// </summary>
        /// <param name="key">Key associated with the value.</param>
        /// <returns></returns>
        bool Remove(K key);
    }
}
