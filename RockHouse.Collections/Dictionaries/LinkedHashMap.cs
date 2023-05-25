using RockHouse.Collections.Dictionaries.Json.SystemTextJson;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// LinkedHashMap is a class that has almost the same functionality as LinkedOrderedDictionary.
    /// This class is intended for programmers familiar with the Java language.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    [JsonConverter(typeof(DictionaryJsonConverterFactory))]
    public class LinkedHashMap<K, V> : LinkedOrderedDictionary<K, V>
    {
        /// <summary>
        /// Constructs an empty instance.
        /// </summary>
        public LinkedHashMap() { }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        public LinkedHashMap(int capacity) : base(capacity) { }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        public LinkedHashMap(IEnumerable<KeyValuePair<K, V>> src) : base(src) { }
    }
}