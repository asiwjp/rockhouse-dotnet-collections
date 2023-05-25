using RockHouse.Collections.Dictionaries.Json.SystemTextJson;
using RockHouse.Collections.Slots;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// LinkedOrderedDictionary is an ordered Dictionary that allows you to retrieve keys and values in the order in which they were registered.
    /// The order is managed by LinkedList.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    [JsonConverter(typeof(DictionaryJsonConverterFactory))]
    public class LinkedOrderedDictionary<K, V> : AbstractOrderedDictionary<K, V> where K : notnull
    {
        private readonly Dictionary<K, Slot<V, LinkedListNode<K>>> _dic;
        private readonly LinkedList<K> _orderedKeys;

        /// <summary>
        /// Constructs an empty instance.
        /// </summary>
        public LinkedOrderedDictionary() : this(0)
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        public LinkedOrderedDictionary(int capacity)
        {
            _dic = new Dictionary<K, Slot<V, LinkedListNode<K>>>(capacity);
            _orderedKeys = new LinkedList<K>();
        }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        public LinkedOrderedDictionary(IEnumerable<KeyValuePair<K, V>> src) : this(0)
        {
            this.AddAll(src);
        }

        /// <inheritdoc/>
        protected void MoveOrderToLast(K key)
        {
            var node = this._dic[key].Item2;
            if (node.Next == null)
            {
                return;
            }
            this._orderedKeys.Remove(node);
            this._orderedKeys.AddLast(node);
        }

        /// <inheritdoc/>
        protected override bool Update(K key, V value)
        {
            if (!this.ContainsKey(key))
            {
                return false;
            }
            this._dic[key].Item1 = value;
            return true;
        }

        #region ICollection
        /// <inheritdoc/>
        public override int Count => _dic.Count;

        /// <inheritdoc/>
        public override bool IsReadOnly => false;

        /// <inheritdoc/>
        public override void Clear()
        {
            _dic.Clear();
            _orderedKeys.Clear();
        }
        #endregion

        #region IDictionary
        /// <inheritdoc/>
        public override ICollection<K> Keys => new ReadOnlyCollection<K>(_orderedKeys.ToList());

        /// <inheritdoc/>
        public override ICollection<V> Values => new ReadOnlyCollection<V>(_orderedKeys.Select(k => _dic[k].Item1).ToList());

        /// <inheritdoc/>
        public override void Add(K key, V value)
        {
            var slot = new Slot<V, LinkedListNode<K>>(value, null);
            _dic.Add(key, slot);
            slot.Item2 = _orderedKeys.AddLast(key);
        }

        /// <inheritdoc/>
        public override bool ContainsKey(K key)
        {
            return _dic.ContainsKey(key);
        }

        /// <inheritdoc/>
        public override bool Remove(K key)
        {
            if (!_dic.TryGetValue(key, out var slot))
            {
                return false;
            }
            _orderedKeys.Remove(slot.Item2);
            _dic.Remove(key);
            return true;
        }

        /// <inheritdoc/>
        public override bool TryGetValue(K key, out V value)
        {
            if (!_dic.TryGetValue(key, out var slot))
            {
                value = default;
                return false;
            }
            value = slot.Item1;
            return true;
        }
        #endregion

        #region IOrderedDictionary
        /// <inheritdoc/>
        public override K FirstKey
        {
            get
            {
                this.CheckEmpty();
                return this._orderedKeys.First.Value;
            }
        }

        /// <inheritdoc/>
        public override K LastKey
        {
            get
            {
                this.CheckEmpty();
                return this._orderedKeys.Last.Value;
            }
        }
        #endregion
    }
}
