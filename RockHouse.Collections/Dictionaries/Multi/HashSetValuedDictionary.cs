using RockHouse.Collections.Dictionaries.Multi.Json.SystemTextJson;
using RockHouse.Collections.Sets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RockHouse.Collections.Dictionaries.Multi
{
    /// <summary>
    /// LinkedHashSetValuedDictionary is a dictionary that allows multiple values to be associated with a single key.
    /// Values are managed by HashSet.
    /// No KeyNotFoundException is thrown in this class. If an attempt is made to retrieve a value using an unregistered key, an empty collection will be returned.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    [JsonConverter(typeof(MultiValuedDictionaryJsonConverterFactory))]
    public class HashSetValuedDictionary<K, V> : AbstractMultiValuedDictionary<K, V, ISet<V>>
    {
        private static readonly HashSet<V> _emptyValues = new HashSet<V>();

        /// <summary>
        /// Constructs an empty instance.
        /// </summary>
        public HashSetValuedDictionary() : base()
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        public HashSetValuedDictionary(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        public HashSetValuedDictionary(IEnumerable<KeyValuePair<K, V>> src) : base(src)
        {
        }

        /// <inheritdoc/>
        protected override ISet<V> EmptyValues => _emptyValues;

        /// <inheritdoc/>
        protected override ISet<V> CreateValues()
        {
            return new LinkedHashSet<V>();
        }

        /// <inheritdoc/>
        protected override ISet<V> CreateValuesProxy(K key)
        {
            return new ValuesProxy(this, key);
        }

        internal class ValuesProxy : AbstractValuesProxy, ISet<V>
        {
            internal ValuesProxy(AbstractMultiValuedDictionary<K, V, ISet<V>> parent, K key) : base(parent, key)
            {
            }

            public void ExceptWith(IEnumerable<V> other)
            {
                this.ValuesForWrite.ExceptWith(other);
                this.CleanupParentEntry();
            }

            public void IntersectWith(IEnumerable<V> other)
            {
                this.ValuesForWrite.IntersectWith(other);
                this.CleanupParentEntry();
            }

            public bool IsProperSubsetOf(IEnumerable<V> other)
            {
                return this.ValuesForRead.IsProperSubsetOf(other);
            }

            public bool IsProperSupersetOf(IEnumerable<V> other)
            {
                return this.ValuesForRead.IsProperSupersetOf(other);
            }

            public bool IsSubsetOf(IEnumerable<V> other)
            {
                return this.ValuesForRead.IsSubsetOf(other);
            }

            public bool IsSupersetOf(IEnumerable<V> other)
            {
                return this.ValuesForRead.IsSupersetOf(other);
            }

            public bool Overlaps(IEnumerable<V> other)
            {
                return this.ValuesForRead.Overlaps(other);
            }

            public bool SetEquals(IEnumerable<V> other)
            {
                return this.ValuesForRead.SetEquals(other);
            }

            public void SymmetricExceptWith(IEnumerable<V> other)
            {
                this.ValuesForWrite.SymmetricExceptWith(other);
                this.CleanupParentEntry();
            }

            public void UnionWith(IEnumerable<V> other)
            {
                this.ValuesForWrite.UnionWith(other);
            }

            bool ISet<V>.Add(V item)
            {
                var values = this.ValuesForWrite;
                var before = values.Count;
                values.Add(item);
                return values.Count != before;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ValuesForRead.GetEnumerator();
            }
        }
    }
}
