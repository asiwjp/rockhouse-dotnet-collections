using System;
using System.Collections;
using System.Collections.Generic;

namespace RockHouse.Collections.Dictionaries
{
    public abstract class AbstractDictionary<K, V> : AbstractCollection, IHashMap<K, V> where K : notnull
    {
        public override bool IsEmpty => this.Count == 0;

        protected abstract bool Update(K key, V value);

        #region ICollection
        public abstract int Count { get; }

        public abstract bool IsReadOnly { get; }

        public void Add(KeyValuePair<K, V> item)
        {
            Add(item.Key, item.Value);
        }

        public abstract void Clear();

        public bool Contains(KeyValuePair<K, V> item)
        {
            if (!TryGetValue(item.Key, out var value))
            {
                return false;
            }
            return object.Equals(item.Value, value);
        }

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
        public V this[K key]
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

        public abstract ICollection<K> Keys { get; }

        public abstract ICollection<V> Values { get; }

        public abstract void Add(K key, V value);

        public abstract bool ContainsKey(K key);

        public abstract bool Remove(K key);

        public abstract bool TryGetValue(K key, out V value);
        #endregion

        #region IEnumerable and Enumerator
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
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
        public override string ToString()
        {
            return "Count=" + this.Count;
        }
        #endregion
    }
}
