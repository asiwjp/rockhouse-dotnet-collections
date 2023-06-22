using RockHouse.Collections.Dictionaries.Json.SystemTextJson;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// HashMap is a class that has almost the same functionality as Dictionary.
    /// This class is intended for programmers familiar with the Java language.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>

    [JsonConverter(typeof(DictionaryJsonConverterFactory))]
    public class HashMap<K, V> : AbstractDictionary<K, V>, IHashMap<K, V> where K : notnull
    {
        private readonly Dictionary<K, V> _dic;

        /// <summary>
        /// Constructs an empty instance.
        /// </summary>
        public HashMap() : this(0, null)
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        public HashMap(int capacity) : this(capacity, null)
        {
        }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        public HashMap(IEnumerable<KeyValuePair<K, V>> src) : this(src, null)
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="comparer">A comparer that compares keys.</param>
        public HashMap(IEqualityComparer<K>? comparer) : this(0, comparer)
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public HashMap(int capacity, IEqualityComparer<K>? comparer)
        {
            this._dic = new Dictionary<K, V>(capacity, comparer);
        }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public HashMap(IEnumerable<KeyValuePair<K, V>> src, IEqualityComparer<K>? comparer) : this(0, comparer)
        {
            this.AddAll(src);
        }

        /// <inheritdoc/>
        protected override bool Update(K key, V value)
        {
            if (!this.ContainsKey(key))
            {
                return false;
            }
            this._dic[key] = value;
            return true;
        }

        #region ICollection
        /// <inheritdoc/>
        public override int Count => _dic.Count;

        /// <inheritdoc/>
        public override bool IsReadOnly => false;

        /// <inheritdoc/>
        public override void Clear() => _dic.Clear();
        #endregion

        #region IDictionary
        /// <inheritdoc/>
        public override ICollection<K> Keys => _dic.Keys;

        /// <inheritdoc/>
        public override ICollection<V> Values => _dic.Values;

        /// <inheritdoc/>
        public override void Add(K key, V value) => _dic.Add(key, value);

        /// <inheritdoc/>
        public override bool ContainsKey(K key) => _dic.ContainsKey(key);

        /// <inheritdoc/>
        public override bool Remove(K key) => _dic.Remove(key);

        /// <inheritdoc/>
        public override bool TryGetValue(K key, out V value) => _dic.TryGetValue(key, out value);
        #endregion
    }
}
