using RockHouse.Collections.Dictionaries.Json.SystemTextJson;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// LinkedHashMap is a class that has almost the same functionality as LruDictionary.
    /// </summary>
    /// <remarks>This class is not thread-safe.</remarks>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    [JsonConverter(typeof(DictionaryJsonConverterFactory))]
    public class LruMap<K, V> : LruDictionary<K, V>
    {
        /// <summary>
        /// Construct by default capacity(100).
        /// Unlike most collection classes, capacity is used as an upper limit on the number of objects that can be stored.
        /// </summary>
        public LruMap() : base(DefaultCapacity, null)
        {
        }

        /// <summary>
        /// Construct by specifying capacity.
        /// Unlike most collection classes, capacity is used as an upper limit on the number of objects that can be stored.
        /// </summary>
        /// <param name="capacity">The maximum number of objects that can be stored.</param>
        public LruMap(int capacity) : base(capacity, null)
        {
        }

        /// <summary>
        /// Construct an object that stores the source elements as the initial value.
        /// Capacity is set equal to the number of source elements.
        /// </summary>
        /// <param name="src">initial elements.</param>
        public LruMap(IEnumerable<KeyValuePair<K, V>> src) : base(src, null)
        {
        }

        /// <summary>
        /// Construct by default capacity(100).
        /// Unlike most collection classes, capacity is used as an upper limit on the number of objects that can be stored.
        /// </summary>
        public LruMap(IEqualityComparer<K>? comparer) : base(DefaultCapacity, comparer)
        {
        }

        /// <summary>
        /// Construct by specifying capacity.
        /// Unlike most collection classes, capacity is used as an upper limit on the number of objects that can be stored.
        /// </summary>
        /// <param name="capacity">The maximum number of objects that can be stored.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public LruMap(int capacity, IEqualityComparer<K>? comparer) : base(capacity, comparer)
        {
        }

        /// <summary>
        /// Construct an object that stores the source elements as the initial value.
        /// Capacity is set equal to the number of source elements.
        /// </summary>
        /// <param name="src">initial elements.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public LruMap(IEnumerable<KeyValuePair<K, V>> src, IEqualityComparer<K>? comparer) : base(src, comparer)
        {
        }
    }
}
