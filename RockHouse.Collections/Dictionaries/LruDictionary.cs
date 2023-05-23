using RockHouse.Collections.Dictionaries.Json.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// LruDictionary is a Dictionary for which a size limit is applied by the Lru algorithm.
    /// </summary>
    /// <remarks>This class is not thread-safe.</remarks>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    [JsonConverter(typeof(DictionaryJsonConverterFactory))]
    public class LruDictionary<K, V> : LinkedOrderedDictionary<K, V>, IDisposable
    {
        /// <summary>
        /// Arguments of the Removed event.
        /// </summary>
        public class LruRemovedEventArgs : EventArgs
        {
            /// <summary>
            /// Removed key.
            /// </summary>
            public K Key { get; }

            /// <summary>
            /// Removed value.
            /// </summary>
            public V Value { get; }

            /// <summary>
            /// Construct EventArgs.
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public LruRemovedEventArgs(K key, V value)
            {
                Key = key;
                Value = value;
            }
        }

        private bool disposedValue;

        /// <summary>
        /// This event is fired after an element is removed.
        /// </summary>
        public event EventHandler<LruRemovedEventArgs> Removed;

        /// <summary>
        /// The maximum number of objects that can be stored.
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// Construct by default settings.
        /// capacity is 100.
        /// </summary>
        public LruDictionary() : this(100)
        {
        }

        /// <summary>
        /// Construct by specifying capacity.
        /// Unlike most collection classes, capacity is used as an upper limit on the number of objects that can be stored.
        /// </summary>
        /// <param name="capacity">The maximum number of objects that can be stored.</param>
        public LruDictionary(int capacity) : base(capacity)
        {
            this.Capacity = capacity;
        }

        /// <summary>
        /// Construct an object that stores the source elements as the initial value.
        /// Capacity is set equal to the number of source elements.
        /// </summary>
        /// <param name="src">initial elements.</param>
        public LruDictionary(IEnumerable<KeyValuePair<K, V>> src)
        {
            foreach (var entry in src)
            {
                this.Capacity++;
                this.Add(entry);
            }
        }

        protected override bool Update(K key, V value)
        {
            var result = base.Update(key, value);
            if (result)
            {
                this.MoveOrderToLast(key);
            }
            return result;
        }

        #region IDictionary
        public override void Add(K key, V value)
        {
            while (!this.IsEmpty && this.Count >= this.Capacity)
            {
                this.Remove(this.FirstKey);
            }
            base.Add(key, value);
        }

        public override bool Remove(K key)
        {
            var result = base.TryGetValue(key, out var value);
            if (result)
            {
                base.Remove(key);
                if (this.Removed != null)
                {
                    this.Removed(this, new LruRemovedEventArgs(key, value));
                }
            }
            return result;
        }

        public override bool TryGetValue(K key, out V value)
        {
            var result = base.TryGetValue(key, out value);
            if (result)
            {
                this.MoveOrderToLast(key);
            }
            return result;
        }
        #endregion

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    while (!this.IsEmpty)
                    {
                        this.Remove(this.FirstKey);
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
