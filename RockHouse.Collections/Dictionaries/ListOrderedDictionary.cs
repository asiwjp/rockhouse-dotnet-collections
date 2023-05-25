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
    /// The order is managed by List.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    [JsonConverter(typeof(DictionaryJsonConverterFactory))]
    public class ListOrderedDictionary<K, V> : AbstractOrderedDictionary<K, V> where K : notnull
    {
        private readonly Dictionary<K, Slot<V, int>> _dic;
        private readonly List<K> _orderedKeys;

        public ListOrderedDictionary() : this(0)
        {
        }

        public ListOrderedDictionary(int capacity)
        {
            _dic = new Dictionary<K, Slot<V, int>>(capacity);
            _orderedKeys = new List<K>(capacity);
        }

        public ListOrderedDictionary(IEnumerable<KeyValuePair<K, V>> src) : this(0)
        {
            this.AddAll(src);
        }

        /// <inheritdoc/>
        protected override bool Update(K key, V value)
        {
            if (!this._dic.TryGetValue(key, out var dicValue))
            {
                return false;
            }

            dicValue.Item1 = value;
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
        public override ICollection<K> Keys => new ReadOnlyCollection<K>(this._orderedKeys.Cast<K>().ToList());

        /// <inheritdoc/>
        public override ICollection<V> Values => new ReadOnlyCollection<V>(this._orderedKeys.Select(k => _dic[k].Item1).ToList());


        /// <inheritdoc/>
        public override void Add(K key, V value)
        {
            int i = this.Count;
            _dic.Add(key, new Slot<V, int>(value, i));
            _orderedKeys.Add(key);
        }

        /// <inheritdoc/>
        public override bool ContainsKey(K key)
        {
            return _dic.ContainsKey(key);
        }

        /// <inheritdoc/>
        public override bool Remove(K key)
        {
            if (!this._dic.TryGetValue(key, out var dicValue))
            {
                return false;
            }

            var lastOfBefore = this._orderedKeys.Count - 1;
            var removeAt = dicValue.Item2;
            _orderedKeys.RemoveAt(removeAt);
            _dic.Remove(key);

            if (removeAt < lastOfBefore)
            {
                this.ReIndex(removeAt);
            }
            return true;
        }

        /// <inheritdoc/>
        public override bool TryGetValue(K key, out V value)
        {
            if (!this._dic.TryGetValue(key, out var dicValue))
            {
                value = default;
                return false;
            }
            value = dicValue.Item1;
            return true;
        }

        /// <inheritdoc/>
        protected void ReIndex(int gapStart)
        {
            for (var i = gapStart; i < _orderedKeys.Count; ++i)
            {
                var key = _orderedKeys[i];
                this._dic[key].Item2 = i;
            }
        }
        #endregion

        #region IOrderedDictionary
        /// <inheritdoc/>
        public override K FirstKey
        {
            get
            {
                this.CheckEmpty();
                return this._orderedKeys.First();
            }
        }

        /// <inheritdoc/>
        public override K LastKey
        {
            get
            {
                this.CheckEmpty();
                return this._orderedKeys.Last();
            }
        }
        #endregion
    }
}
