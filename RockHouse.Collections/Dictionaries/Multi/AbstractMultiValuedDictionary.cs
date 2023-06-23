using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RockHouse.Collections.Dictionaries.Multi
{
    /// <summary>
    /// An abstract implementation of a IMultiValuedMap. 
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    /// <typeparam name="C">The type of the value collection.</typeparam>
    public abstract class AbstractMultiValuedDictionary<K, V, C> : AbstractCollection, IMultiValuedMap<K, V, C>
        where K : notnull
        where C : ICollection<V>
    {
        private readonly Dictionary<K, C> _dic;

        /// <summary>
        /// should be a ReadOnly collection.
        /// </summary>
        protected abstract C EmptyValues { get; }

        /// <summary>
        /// Constructs an empty instance.
        /// </summary>
        public AbstractMultiValuedDictionary() : this(0, null)
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        public AbstractMultiValuedDictionary(int capacity) : this(capacity, null)
        {
        }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        public AbstractMultiValuedDictionary(IEnumerable<KeyValuePair<K, V>> src) : this(src, null)
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="comparer">A comparer that compares keys.</param>
        public AbstractMultiValuedDictionary(IEqualityComparer<K>? comparer) : this(0, comparer)
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public AbstractMultiValuedDictionary(int capacity, IEqualityComparer<K>? comparer)
        {
            _dic = new Dictionary<K, C>(capacity, comparer);
        }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public AbstractMultiValuedDictionary(IEnumerable<KeyValuePair<K, V>> src, IEqualityComparer<K>? comparer) : this(0, comparer)
        {
            foreach (var item in src)
            {
                this.Add(item);
            }
        }

        /// <inheritdoc/>
        public override bool IsEmpty => this._dic.Count == 0;

        /// <summary>
        /// Clean the entry corresponding to the key.
        /// </summary>
        /// <param name="key"></param>
        protected void CleanupEntry(K key)
        {
            if (this._dic[key].Count == 0)
            {
                this.Remove(key);
            }
        }

        /// <summary>
        /// Create a collection of values to be associated with a key.
        /// </summary>
        /// <returns></returns>
        protected abstract C CreateValues();

        /// <summary>
        /// Create a proxy for the collection of values to be associated with the key.
        /// </summary>
        /// <returns></returns>
        protected abstract C CreateValuesProxy(K key);

        internal C ReadyInternalValues(K key)
        {
            if (_dic.TryGetValue(key, out var values))
            {
                return values;
            }

            values = this.CreateValues();
            this._dic.Add(key, values);
            return values;
        }

        internal C InternalValuesForRead(K key)
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
        public C this[K key]
        {
            get
            {
                return CreateValuesProxy(key);
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
        public bool AddAll(K key, IEnumerable<V> src)
        {
            var result = false;
            foreach (var item in src)
            {
                result |= this.Put(key, item);
            }
            return result;
        }

        /// <inheritdoc/>
        public bool AddAll(IEnumerable<KeyValuePair<K, V>> src)
        {
            var result = false;
            foreach (var item in src)
            {
                result |= this.Put(item.Key, item.Value);
            }
            return result;
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
        public bool TryGetValue(K key, out C value)
        {
            value = this.CreateValuesProxy(key);
            return this.ContainsKey(key);
        }
        #endregion

        #region IMultiValuedMap
        /// <inheritdoc/>
        public C Get(K key) => this[key];

        /// <inheritdoc/>
        public C Delete(K key)
        {
            var result = this.CreateValues();
            foreach (var e in this[key])
            {
                result.Add(e);
            }
            this.Remove(key);
            return result;
        }

        /// <inheritdoc/>
        public bool Put(K key, V value) => this.Add(key, value);

        /// <inheritdoc/>
        public bool PutAll(K key, IEnumerable<V> src) => this.AddAll(key, src);

        /// <inheritdoc/>
        public bool PutAll(IEnumerable<KeyValuePair<K, V>> src) => this.AddAll(src);
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

        internal class AbstractValuesProxy
        {
            protected AbstractValuesProxy(AbstractMultiValuedDictionary<K, V, C> parent, K key)
            {
                this.ParentRef = new WeakReference<AbstractMultiValuedDictionary<K, V, C>>(parent);
                this.Key = key;
            }

            protected K Key { get; }

            protected WeakReference<AbstractMultiValuedDictionary<K, V, C>> ParentRef { get; }

            protected AbstractMultiValuedDictionary<K, V, C> Parent
            {
                get
                {
                    if (this.ParentRef.TryGetTarget(out var parent))
                    {
                        return parent;
                    }
                    throw new InvalidOperationException();
                }
            }

            protected C ValuesForWrite => this.Parent.ReadyInternalValues(Key);

            protected C ValuesForRead => this.Parent.InternalValuesForRead(Key);

            protected void CleanupParentEntry()
            {
                this.Parent.CleanupEntry(this.Key);
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
            public bool Remove(V item)
            {
                var result = this.ValuesForWrite.Remove(item);
                this.CleanupParentEntry();
                return result;
            }

        }
    }
}