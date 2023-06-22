using RockHouse.Collections.Dictionaries.Json.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// ReferenceDictionary is a dictionary that allows deletion of entries by the GC.
    /// 
    /// This class does not support Jsonization.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    [JsonConverter(typeof(DictionaryJsonConverterFactory))]
    public class ReferenceDictionary<K, V> : AbstractDictionary<K, V>, IHashMap<K, V>
    {
        private HashMap<int, LinkedList<ReferenceEntry<K, V>>> _dic;
        private IEqualityComparer<K> _equalityComparer;

        /// <summary>
        /// The reference strength of keys.
        /// </summary>
        public ReferenceStrength KeyReferenceStrength { get; }

        /// <summary>
        /// The reference strength of values.
        /// </summary>
        public ReferenceStrength ValueReferenceStrength { get; }

        /// <summary>
        /// Construct an instance that hold keys by strong reference and values by weak reference.
        /// </summary>
        public ReferenceDictionary() : this(ReferenceStrength.Hard, ReferenceStrength.Weak, 0, null)
        {
        }

        /// <summary>
        /// Construct an instance that hold keys by strong reference and values by weak reference.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        public ReferenceDictionary(int capacity) : this(ReferenceStrength.Hard, ReferenceStrength.Weak, capacity, null)
        {
        }

        /// <summary>
        /// Constructs an instance that holds keys and values with the specified reference strength.
        /// </summary>
        /// <param name="keyReferenceStrength">The reference strength of keys.</param>
        /// <param name="valueReferenceStrength">The reference strength of values.</param>
        /// <param name="capacity">Initial capacity of the collection.</param>
        public ReferenceDictionary(ReferenceStrength keyReferenceStrength, ReferenceStrength valueReferenceStrength, int capacity) : this(keyReferenceStrength, valueReferenceStrength, capacity, null)
        {
        }

        /// <summary>
        /// Construct an object that stores the source elements as the initial value.
        /// Elements are keyed by strong references and valued by weak references.
        /// </summary>
        /// <param name="src">initial elements.</param>
        public ReferenceDictionary(IEnumerable<KeyValuePair<K, V>> src) : this(src, null)
        {
        }

        /// <summary>
        /// Construct an instance that hold keys by strong reference and values by weak reference.
        /// </summary>
        /// <param name="comparer">A comparer that compares keys.</param>
        public ReferenceDictionary(IEqualityComparer<K>? comparer) : this(ReferenceStrength.Hard, ReferenceStrength.Weak, 0, comparer)
        {
        }

        /// <summary>
        /// Construct an instance that hold keys by strong reference and values by weak reference.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public ReferenceDictionary(int capacity, IEqualityComparer<K>? comparer) : this(ReferenceStrength.Hard, ReferenceStrength.Weak, capacity, comparer)
        {
        }

        /// <summary>
        /// Constructs an instance that holds keys and values with the specified reference strength.
        /// </summary>
        /// <param name="keyReferenceStrength">The reference strength of keys.</param>
        /// <param name="valueReferenceStrength">The reference strength of values.</param>
        /// <param name="capacity">Initial capacity of the collection.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public ReferenceDictionary(ReferenceStrength keyReferenceStrength, ReferenceStrength valueReferenceStrength, int capacity, IEqualityComparer<K>? comparer)
        {
            _dic = new HashMap<int, LinkedList<ReferenceEntry<K, V>>>(capacity);
            _equalityComparer = comparer ?? EqualityComparer<K>.Default;
            KeyReferenceStrength = keyReferenceStrength;
            ValueReferenceStrength = valueReferenceStrength;
        }

        /// <summary>
        /// Construct an object that stores the source elements as the initial value.
        /// Capacity is set equal to the number of source elements.
        /// Elements are keyed by strong references and valued by weak references.
        /// </summary>
        /// <param name="src">initial elements.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public ReferenceDictionary(IEnumerable<KeyValuePair<K, V>> src, IEqualityComparer<K>? comparer) : this(ReferenceStrength.Hard, ReferenceStrength.Weak, 0, comparer)
        {
            this.AddAll(src);
        }

        /// <summary>
        /// Scans all entries in the dictionary and returns only those with surviving instances.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<KeyValuePair<K, V>> Scan()
        {
            if (_dic.Count == 0)
            {
                yield break;
            }

            foreach (var keyHash in _dic.Keys.ToList())
            {
                foreach (var entry in ScanByKeyHash(keyHash))
                {
                    yield return entry;
                }
            }
        }

        /// <summary>
        /// Scans the entries for a given key in the dictionary and returns only those with surviving instances.
        /// </summary>
        /// <param name="key">Scan key</param>
        /// <returns></returns>
        private IEnumerable<KeyValuePair<K, V>> ScanByKey(K key)
        {
            var keyHash = _equalityComparer.GetHashCode(key);
            return ScanByKeyHash(keyHash)
                .Where(e => _equalityComparer.Equals(e.Key, key));
        }

        /// <summary>
        /// Scans the entries for a given key hash in the dictionary and returns only those with surviving instances.
        /// </summary>
        /// <param name="keyHash">Scan key</param>
        /// <returns></returns>
        private IEnumerable<KeyValuePair<K, V>> ScanByKeyHash(int keyHash)
        {
            if (!_dic.TryGetValue(keyHash, out var bucket))
            {
                yield break;
            }

            var phantoms = new List<LinkedListNode<ReferenceEntry<K, V>>>();
            var node = bucket.First;
            do
            {
                var kv = node.Value.GetKeyValue();
                if (kv != null)
                {
                    yield return kv.Value;
                }
                else
                {
                    phantoms.Add(node);
                }
                node = node.Next;
            } while (node != null);

            foreach (var phantom in phantoms)
            {
                bucket.Remove(phantom);
            }
            if (bucket.Count == 0)
            {
                _dic.Remove(keyHash);
            }
        }

        /// <inheritdoc/>
        protected override bool Update(K key, V value)
        {
            var keyHash = _equalityComparer.GetHashCode(key);
            if (!_dic.TryGetValue(keyHash, out var bucket))
            {
                return false;
            }

            foreach (var entry in bucket)
            {
                if (entry.Key.Equals(key))
                {
                    entry.Value.Set(value);
                    return true;
                }
            }
            return false;
        }

        #region ICollection
        /// <inheritdoc/>
        public override int Count => _dic.Count;

        /// <inheritdoc/>
        public override bool IsReadOnly => false;
        #endregion


        #region IDictionary
        /// <inheritdoc/>
        public override ICollection<K> Keys
        {
            get
            {
                return Scan().Select(e => e.Key).ToList().AsReadOnly();
            }
        }

        /// <inheritdoc/>
        public override ICollection<V> Values
        {
            get
            {
                return Scan().Select(e => e.Value).ToList().AsReadOnly();
            }
        }

        /// <inheritdoc/>
        public override void Add(K key, V value)
        {
            var keyHash = _equalityComparer.GetHashCode(key);
            var bucket = _dic.Get(keyHash, ifNotFound: (hash) =>
            {
                var bucket = new LinkedList<ReferenceEntry<K, V>>();
                _dic.Add(hash, bucket);
                return bucket;
            });
            if (bucket.Any(e => e.Key.Equals(key)))
            {
                throw new ArgumentException($"The specified key already exists.");
            }

            var entry = new ReferenceEntry<K, V>(this.KeyReferenceStrength, key, this.ValueReferenceStrength, value, _equalityComparer);
            bucket.AddLast(entry);
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            _dic.Clear();
        }

        /// <inheritdoc/>
        public override bool ContainsKey(K key)
        {
            return ScanByKey(key).Any();
        }

        /// <inheritdoc/>
        public override bool Remove(K key)
        {
            var keyHash = _equalityComparer.GetHashCode(key);
            if (!_dic.TryGetValue(keyHash, out var bucket))
            {
                return false;
            }
            var node = bucket.First;
            while (node != null)
            {
                if (node.Value.Key.Equals(key))
                {
                    bucket.Remove(node);
                    if (bucket.Count == 0)
                    {
                        _dic.Remove(keyHash);
                    }
                    return true;
                }
                node = node.Next;
            }
            return false;
        }

        /// <inheritdoc/>
        public override bool TryGetValue(K key, out V value)
        {
            var result = ScanByKey(key);
            value = result.FirstOrDefault().Value;
            return result.Any();
        }
        #endregion

        #region IEnumerable
        /// <inheritdoc/>
        public override IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return this.Scan().GetEnumerator();
        }
        #endregion
    }
}
