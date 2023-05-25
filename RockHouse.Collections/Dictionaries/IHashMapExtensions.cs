using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// Provides extension methods for IHashMap.
    /// </summary>
    public static class IHashMapExtensions
    {
        /// <summary>
        /// Copy all elements into the collection. Duplicate keys are not allowed.
        /// </summary>
        /// <param name="map">Collection to be extended.</param>
        /// <param name="src">The enumerable from which to copy.</param>
        public static void AddAll<K, V>(this IHashMap<K, V> map, IEnumerable<KeyValuePair<K, V>> src)
        {
            foreach (var item in src)
            {
                map.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="map">Collection to be extended.</param>
        /// <param name="key">Key associated with the value.</param>
        /// <returns>The value associated with the key is returned. If the key is not found, default(V).</returns>
        public static V Get<K, V>(this IHashMap<K, V> map, K key)
        {
            if (map.TryGetValue(key, out var value))
            {
                return value;
            }

            return default;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="K">The type of the key.</typeparam>
        /// <typeparam name="V">The type of the value.</typeparam>
        /// <param name="map">Collection to be extended.</param>
        /// <param name="key">Key associated with the value.</param>
        /// <param name="ifNotFound">A callback function that is called if the key is not found. The function is passed a key, and the return value is used as an alternative to the method return value.</param>
        /// <param name="ifFound">A callback function that is called if the key is found. The function is passed a key and an old value, and the return value is used as an alternative to the method return value.</param>
        /// <returns>The value associated with the key is returned. If the key is not found, default(V). In either case, if a callback function is specified, it will be replaced by its return value.</returns>
        public static V Get<K, V>(this IHashMap<K, V> map, K key, Func<K, V> ifNotFound = null, Func<K, V, V> ifFound = null)
        {
            if (map.TryGetValue(key, out var value))
            {
                if (ifFound != null)
                {
                    return ifFound(key, value);
                }
                return value;
            }

            if (ifNotFound != null)
            {
                return ifNotFound(key);
            }
            return default;
        }

        /// <summary>
        /// Copy all elements into the collection. If duplicate keys are found, the last element is stored.
        /// </summary>
        /// <param name="map">Collection to be extended.</param>
        /// <param name="src">The enumerable from which to copy.</param>
        public static void PutAll<K, V>(this IHashMap<K, V> map, IEnumerable<KeyValuePair<K, V>> src)
        {
            foreach (var item in src)
            {
                map.Put(item.Key, item.Value);
            }
        }
    }
}
