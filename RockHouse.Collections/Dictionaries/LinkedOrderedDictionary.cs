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

        public LinkedOrderedDictionary() : this(0)
        {
        }

        public LinkedOrderedDictionary(int capacity)
        {
            _dic = new Dictionary<K, Slot<V, LinkedListNode<K>>>(capacity);
            _orderedKeys = new LinkedList<K>();
        }

        public LinkedOrderedDictionary(IEnumerable<KeyValuePair<K, V>> src) : this(0)
        {
            this.AddAll(src);
        }

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
        public override int Count => _dic.Count;

        public override bool IsReadOnly => false;

        public override void Clear()
        {
            _dic.Clear();
            _orderedKeys.Clear();
        }
        #endregion

        #region IDictionary
        public override ICollection<K> Keys => new ReadOnlyCollection<K>(_orderedKeys.ToList());

        public override ICollection<V> Values => new ReadOnlyCollection<V>(_orderedKeys.Select(k => this[k]).ToList());

        public override void Add(K key, V value)
        {
            var slot = new Slot<V, LinkedListNode<K>>(value, null);
            _dic.Add(key, slot);
            slot.Item2 = _orderedKeys.AddLast(key);
        }

        public override bool ContainsKey(K key)
        {
            return _dic.ContainsKey(key);
        }

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
        public override K FirstKey
        {
            get
            {
                this.CheckEmpty();
                return this._orderedKeys.First.Value;
            }
        }

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
