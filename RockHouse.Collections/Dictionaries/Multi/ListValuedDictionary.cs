using RockHouse.Collections.Dictionaries.Multi.Json.SystemTextJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries.Multi
{
    /// <summary>
    /// ListValuedDictionary is a dictionary that allows multiple values to be associated with a single key.
    /// Values are managed by List.
    /// No KeyNotFoundException is thrown in this class. If an attempt is made to retrieve a value using an unregistered key, an empty collection will be returned.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    [JsonConverter(typeof(MultiValuedDictionaryJsonConverterFactory))]
    public class ListValuedDictionary<K, V> : AbstractCollection, IMultiValuedMap<K, V, IList<V>> where K : notnull
    {
        private static readonly IList<V> EmptyValues = new List<V>().AsReadOnly();
        private readonly Dictionary<K, IList<V>> _dic;

        /// <summary>
        /// Constructs an empty instance.
        /// </summary>
        public ListValuedDictionary() : this(0)
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        public ListValuedDictionary(int capacity)
        {
            _dic = new Dictionary<K, IList<V>>(capacity);
        }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        public ListValuedDictionary(IEnumerable<KeyValuePair<K, V>> src) : this(0)
        {
            foreach (var item in src)
            {
                this.Add(item);
            }
        }

        /// <inheritdoc/>
        public override bool IsEmpty => this._dic.Count == 0;

        internal void CleanupEntry(K key)
        {
            if (this._dic[key].Count == 0)
            {
                this.Remove(key);
            }
        }

        /// <inheritdoc/>
        public IList<V> Get(K key) => this[key];

        /// <inheritdoc/>
        public bool Put(K key, V value) => this.Add(key, value);

        internal IList<V> ReadyInternalValues(K key)
        {
            if (_dic.TryGetValue(key, out var values))
            {
                return values;
            }

            values = new List<V>();
            this._dic.Add(key, values);
            return values;
        }

        internal IList<V> InternalValuesForRead(K key)
        {
            if (_dic.TryGetValue(key, out var values))
            {
                return values;
            }

            return EmptyValues;
        }

        #region ICollection

        /// <inheritdoc/>
        public int Count => this.Values.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public void Add(KeyValuePair<K, V> entry)
        {
            this.ReadyInternalValues(entry.Key).Add(entry.Value);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _dic.Clear();
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            var availables = array.Length - arrayIndex;
            if (availables < this.Count)
            {
                throw new ArgumentException(nameof(arrayIndex), $"array is too small. required={this.Count}, availables(based on arrayIndex)={availables}, arrayIndex={arrayIndex}");
            }

            foreach (var entry in this)
            {
                array[arrayIndex] = entry;
                arrayIndex++;
            }
        }

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<K, V> item)
        {
            if (!_dic.TryGetValue(item.Key, out var valuesOfKey))
            {
                return false;
            }
            return valuesOfKey.Contains(item.Value);
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<K, V> item)
        {
            if (!_dic.TryGetValue(item.Key, out var values))
            {
                return false;
            }
            if (!values.Remove(item.Value))
            {
                return false;
            }
            this.CleanupEntry(item.Key);
            return true;
        }
        #endregion

        #region IMultiValuedDictionary
        /// <inheritdoc/>
        public IList<V> this[K key]
        {
            get
            {
                return new ValuesProxy(this, key);
            }
            set
            {
                var values = this.ReadyInternalValues(key);
                values.Clear();
                foreach (var v in value)
                {
                    values.Add(v);
                }
            }
        }

        /// <inheritdoc/>
        public ICollection<K> Keys => _dic.Keys;

        /// <inheritdoc/>
        public ICollection<V> Values => _dic.Values.SelectMany(e => e).ToList().AsReadOnly();

        /// <inheritdoc/>
        public bool Add(K key, V value)
        {
            this.ReadyInternalValues(key).Add(value);
            return true;
        }

        /// <inheritdoc/>
        public bool ContainsKey(K key)
        {
            return _dic.ContainsKey(key);
        }

        /// <inheritdoc/>

        public bool Remove(K key)
        {
            return _dic.Remove(key);
        }

        /// <inheritdoc/>
        public bool TryGetValue(K key, out IList<V> value)
        {
            value = new ValuesProxy(this, key);
            return this.ContainsKey(key);
        }
        #endregion

        #region IEnumerable
        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach (var keyValues in this._dic)
            {
                foreach (var value in keyValues.Value)
                {
                    yield return new KeyValuePair<K, V>(keyValues.Key, value);
                }
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region Object
        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Count={Count}";
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        internal class ValuesProxy : IList<V>
        {
            private readonly WeakReference<ListValuedDictionary<K, V>> _parentRef;
            private K _key;

            internal ValuesProxy(ListValuedDictionary<K, V> parent, K key)
            {
                _parentRef = new WeakReference<ListValuedDictionary<K, V>>(parent);
                this._key = key;
            }

            private void CleanupParentEntry()
            {
                this.Parent.CleanupEntry(this._key);
            }

            private ListValuedDictionary<K, V> Parent
            {
                get
                {
                    if (this._parentRef.TryGetTarget(out var parent))
                    {
                        return parent;
                    }
                    throw new InvalidOperationException();
                }
            }

            private IList<V> ValuesForWrite => this.Parent.ReadyInternalValues(_key);

            private IList<V> ValuesForRead => this.Parent.InternalValuesForRead(_key);

            public V this[int index]
            {
                get => this.ValuesForRead[index];
                set => this.ValuesForWrite[index] = value;
            }

            /// <inheritdoc/>
            public int Count => this.ValuesForRead.Count;

            /// <inheritdoc/>
            public bool IsReadOnly => false;

            /// <inheritdoc/>
            public void Add(V item)
            {
                this.ValuesForWrite.Add(item);
            }

            /// <inheritdoc/>
            public void Clear()
            {
                this.ValuesForWrite.Clear();
                this.CleanupParentEntry();
            }

            /// <inheritdoc/>
            public bool Contains(V item)
            {
                return this.ValuesForRead.Contains(item);
            }

            /// <inheritdoc/>
            public void CopyTo(V[] array, int arrayIndex)
            {
                this.ValuesForRead.CopyTo(array, arrayIndex);
            }

            /// <inheritdoc/>
            public IEnumerator<V> GetEnumerator()
            {
                return this.ValuesForRead.GetEnumerator();
            }

            /// <inheritdoc/>
            public int IndexOf(V item)
            {
                return this.ValuesForRead.IndexOf(item);
            }

            /// <inheritdoc/>
            public void Insert(int index, V item)
            {
                this.ValuesForWrite.Insert(index, item);
            }

            /// <inheritdoc/>
            public bool Remove(V item)
            {
                var result = this.ValuesForWrite.Remove(item);
                this.CleanupParentEntry();
                return result;
            }

            /// <inheritdoc/>
            public void RemoveAt(int index)
            {
                this.ValuesForWrite.RemoveAt(index);
                this.CleanupParentEntry();
            }

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.ValuesForRead.GetEnumerator();
            }
        }
    }
}