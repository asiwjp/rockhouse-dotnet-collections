using RockHouse.Collections.Dictionaries.Json.SystemTextJson;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// LruMap is a Dictionary for which a size limit is applied by the Lru algorithm.
    /// </summary>
    /// <remarks>This class is thread-safe.</remarks>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    [JsonConverter(typeof(DictionaryJsonConverterFactory))]
    public class LruMap<K, V> : LruDictionary<K, V>
    {
        /// <summary>
        /// Construct by default settings.
        /// capacity is 100.
        /// </summary>
        public LruMap() : base(100)
        {
        }

        /// <summary>
        /// Construct by specifying capacity.
        /// Unlike most collection classes, capacity is used as an upper limit on the number of objects that can be stored.
        /// </summary>
        /// <param name="capacity">The maximum number of objects that can be stored.</param>
        public LruMap(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Construct an object that stores the source elements as the initial value.
        /// Capacity is set equal to the number of source elements.
        /// </summary>
        /// <param name="src">initial elements.</param>
        public LruMap(IEnumerable<KeyValuePair<K, V>> src) : base(src)
        {
        }
    }
}
