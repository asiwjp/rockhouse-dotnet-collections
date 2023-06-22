using RockHouse.Collections.Dictionaries.Multi.Json.SystemTextJson;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries.Multi
{
    /// <summary>
    /// ListValuedMap is a class that has almost the same functionality as ListValuedDictionary.
    /// This class is intended for programmers familiar with the Java language.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    [JsonConverter(typeof(MultiValuedDictionaryJsonConverterFactory))]
    public class ListValuedMap<K, V> : ListValuedDictionary<K, V> where K : notnull
    {
        /// <summary>
        /// Constructs an empty instance.
        /// </summary>
        public ListValuedMap() : base(0, null)
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        public ListValuedMap(int capacity) : base(capacity, null)
        {
        }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        public ListValuedMap(IEnumerable<KeyValuePair<K, V>> src) : base(src, null)
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="comparer">A comparer that compares keys.</param>
        public ListValuedMap(IEqualityComparer<K>? comparer) : base(0, comparer)
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public ListValuedMap(int capacity, IEqualityComparer<K>? comparer) : base(capacity, comparer)
        {
        }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public ListValuedMap(IEnumerable<KeyValuePair<K, V>> src, IEqualityComparer<K>? comparer) : base(src, comparer)
        {
        }

    }
}
