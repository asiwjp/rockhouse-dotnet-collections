using System.Collections.Generic;

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
        /// Copy all elements into the collection. If duplicate keys are found, the last element is stored.
        /// </summary>
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
