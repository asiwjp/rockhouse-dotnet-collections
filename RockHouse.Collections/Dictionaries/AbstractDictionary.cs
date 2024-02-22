using System;
using System.Collections;
using System.Collections.Generic;

namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// An abstract implementation of a IDictionary/IHashMap. 
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    public abstract class AbstractDictionary<K, V> : AbstractCollection, IHashMap<K, V> where K : notnull
    {
        /// <inheritdoc/>
        public override bool IsEmpty => this.Count == 0;

        /// <inheritdoc/>
        public virtual V Delete(K key)
        {
            if (this.TryGetValue(key, out V remvoedValue))
            {
                this.Remove(key);
                return remvoedValue;
            }
            return default;
        }

        /// <inheritdoc/>
        public virtual V Delete(K key, Func<K, V> ifNotFound = null, Func<K, V, V> ifFound = null)
        {
            if (this.TryGetValue(key, out V remvoedValue))
            {
                this.Remove(key);
                if (ifFound != null)
                {
                    return ifFound(key, remvoedValue);
                }
                return remvoedValue;
            }

            if (ifNotFound != null)
            {
                return ifNotFound(key);
            }
            return default;
        }

        /// <inheritdoc/>
        public virtual V Put(K key, V value)
        {
            if (this.TryGetValue(key, out var oldValue))
            {
                this[key] = value;
                return oldValue;
            }

            this.Add(key, value);
            return oldValue;
        }

        /// <inheritdoc/>
        public virtual V Put(K key, V value, Func<K, V> ifNotFound = null, Func<K, V, V> ifFound = null)
        {
            if (this.TryGetValue(key, out var oldValue))
            {
                this[key] = value;
                if (ifNotFound != null)
                {
                    return ifFound(key, oldValue);
                }
                return oldValue;
            }

            this.Add(key, value);
            if (ifNotFound != null)
            {
                return ifNotFound(key);
            }
            return oldValue;
        }

        /// <inheritdoc/>
        public virtual V PutIfAbsent(K key, V value)
        {
            if (!this.TryGetValue(key, out var oldValue))
            {
                this.Add(key, value);
            }
            return oldValue;
        }

        /// <inheritdoc/>
        public virtual V Replace(K key, V value)
        {
            if (this.TryGetValue(key, out var oldValue))
            {
                this.Update(key, value);
            }
            return oldValue;
        }

        /// <inheritdoc/>
        protected abstract bool Update(K key, V value);

        #region ICollection
        /// <inheritdoc/>
        public abstract int Count { get; }

        /// <inheritdoc/>
        public abstract bool IsReadOnly { get; }

        /// <inheritdoc/>
        public void Add(KeyValuePair<K, V> item)
        {
            Add(item.Key, item.Value);
        }

        /// <inheritdoc/>
        public abstract void Clear();

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<K, V> item)
        {
            if (!TryGetValue(item.Key, out var value))
            {
                return false;
            }
            return object.Equals(item.Value, value);
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
        public bool Remove(KeyValuePair<K, V> item)
        {
            if (!TryGetValue(item.Key, out V value))
            {
                return false;
            }
            if (!object.Equals(value, item.Value))
            {
                return false;
            }
            return this.Remove(item.Key);
        }
        #endregion


        #region IDictionary
        /// <inheritdoc/>
        public virtual V this[K key]
        {
            get
            {
                if (this.TryGetValue(key, out var value))
                {
                    return value;
                }
                throw new KeyNotFoundException();
            }
            set
            {
                if (!Update(key, value))
                {
                    Add(key, value);
                }
            }
        }

        /// <inheritdoc/>
        public abstract ICollection<K> Keys { get; }

        /// <inheritdoc/>
        public abstract ICollection<V> Values { get; }

        /// <inheritdoc/>
        public abstract void Add(K key, V value);

        /// <inheritdoc/>
        public abstract bool ContainsKey(K key);

        /// <inheritdoc/>
        public abstract bool Remove(K key);

        /// <inheritdoc/>
        public abstract bool TryGetValue(K key, out V value);
        #endregion

        #region IEnumerable and Enumerator
        /// <inheritdoc/>
        public virtual IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach (var key in Keys)
            {
                yield return new KeyValuePair<K, V>(key, this[key]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region Object
        /// <inheritdoc/>
        public override string ToString()
        {
            return "Count=" + this.Count;
        }
        #endregion
    }
}
