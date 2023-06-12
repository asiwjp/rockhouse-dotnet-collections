using RockHouse.Collections.Dictionaries.Multi.Json.SystemTextJson;
using System.Collections;
using System.Collections.Generic;
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
    public class ListValuedDictionary<K, V> : AbstractMultiValuedDictionary<K, V, IList<V>> where K : notnull
    {
        /// <inheritdoc/>
        protected override IList<V> EmptyValues { get; } = new List<V>().AsReadOnly();

        /// <summary>
        /// Constructs an empty instance.
        /// </summary>
        public ListValuedDictionary() : base()
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        public ListValuedDictionary(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        public ListValuedDictionary(IEnumerable<KeyValuePair<K, V>> src) : base(src)
        {
        }

        /// <inheritdoc/>
        protected override IList<V> CreateValues()
        {
            return new List<V>();
        }

        /// <inheritdoc/>
        protected override IList<V> CreateValuesProxy(K key)
        {
            return new ValuesProxy(this, key);
        }

        /// <summary>
        /// 
        /// </summary>
        internal class ValuesProxy : AbstractValuesProxy, IList<V>
        {
            internal ValuesProxy(AbstractMultiValuedDictionary<K, V, IList<V>> parent, K key) : base(parent, key)
            {
            }
            public V this[int index]
            {
                get => this.ValuesForRead[index];
                set => this.ValuesForWrite[index] = value;
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