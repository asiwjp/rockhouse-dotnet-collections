using RockHouse.Collections.Dictionaries.Json.SystemTextJson;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// A WeakHashMap is an imitation of the Java language's WeakHashMap class.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    [JsonConverter(typeof(DictionaryJsonConverterFactory))]
    public class WeakHashMap<K, V> : ReferenceDictionary<K, V>
    {
        /// <summary>
        /// Construct an instance that hold keys by weak reference and values by hard reference.
        /// </summary>
        public WeakHashMap() : base(ReferenceStrength.Weak, ReferenceStrength.Hard, 0, null)
        {
        }

        /// <summary>
        /// Construct an instance that hold keys by weak reference and values by hard reference.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        public WeakHashMap(int capacity) : base(ReferenceStrength.Weak, ReferenceStrength.Hard, capacity, null)
        {
        }

        /// <summary>
        /// Construct an object that stores the source elements as the initial value.
        /// Capacity is set equal to the number of source elements.
        /// </summary>
        /// <param name="src">initial elements.</param>
        public WeakHashMap(IEnumerable<KeyValuePair<K, V>> src) : base(ReferenceStrength.Weak, ReferenceStrength.Hard, 0, null)
        {
            this.AddAll(src);
        }

        /// <summary>
        /// Construct an instance that hold keys by weak reference and values by hard reference.
        /// </summary>
        /// <param name="comparer">A comparer that compares keys.</param>
        public WeakHashMap(IEqualityComparer<K>? comparer) : base(ReferenceStrength.Weak, ReferenceStrength.Hard, 0, comparer)
        {
        }

        /// <summary>
        /// Construct an instance that hold keys by weak reference and values by hard reference.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public WeakHashMap(int capacity, IEqualityComparer<K>? comparer) : base(ReferenceStrength.Weak, ReferenceStrength.Hard, capacity, comparer)
        {
        }

        /// <summary>
        /// Construct an object that stores the source elements as the initial value.
        /// Capacity is set equal to the number of source elements.
        /// </summary>
        /// <param name="src">initial elements.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public WeakHashMap(IEnumerable<KeyValuePair<K, V>> src, IEqualityComparer<K>? comparer) : base(ReferenceStrength.Weak, ReferenceStrength.Hard, 0, comparer)
        {
            this.AddAll(src);
        }
    }
}
