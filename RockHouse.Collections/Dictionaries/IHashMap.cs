using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        /// <summary>
        /// Stores the value associated with the specified key and returns the old value.
        /// If the key already exists, it will be associated to the new value.
        /// </summary>
        /// <param name="key">Key associated with the value.</param>
        /// <param name="value">Value associated with the specified key.</param>
        /// <param name="ifNotFound">A callback function that is called if the key is not found. The function is passed a key, and the return value is used as an alternative to the method return value.</param>
        /// <param name="ifFound">A callback function that is called if the key is found. The function is passed a key and an old value, and the return value is used as an alternative to the method return value.</param>
        /// <returns>The old value associated with the key is returned. If the key is not found, default(V). In either case, if a callback function is specified, it will be replaced by its return value.</returns>
        public V Put(K key, V value, Func<K, V> ifNotFound = null, Func<K, V, V> ifFound = null);

        /// <summary>
        /// Removes the value associated with the specified key and returns the removed value.
        /// If key is not not found, nothing is done.
        /// </summary>
        /// <param name="key">Key associated with the value.</param>
        /// <returns>The removed value associated with the key is returned. If the key is not found, default(V).</returns>
        public V Delete(K key);

        /// <summary>
        /// Removes the value associated with the specified key and returns the removed value.
        /// If key is not not found, nothing is done.
        /// </summary>
        /// <param name="key">Key associated with the value.</param>
        /// <param name="ifNotFound">A callback function that is called if the key is not found. The function is passed a key, and the return value is used as an alternative to the method return value.</param>
        /// <param name="ifFound">A callback function that is called if the key is found. The function is passed a key and an old value, and the return value is used as an alternative to the method return value.</param>
        /// <returns>The old value associated with the key is returned. If the key is not found, default(V). In either case, if a callback function is specified, it will be replaced by its return value.</returns>
        public V Delete(K key, Func<K, V> ifNotFound = null, Func<K, V, V> ifFound = null);
    }
}
