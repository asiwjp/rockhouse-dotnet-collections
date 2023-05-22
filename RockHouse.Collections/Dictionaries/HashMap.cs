using RockHouse.Collections.Dictionaries.Json.SystemTextJson;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// HashMap is a class that has almost the same functionality as Dictionary.
    /// This class is intended for programmers familiar with the Java language.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>

    [JsonConverter(typeof(DictionaryJsonConverterFactory))]
    public class HashMap<K, V> : AbstractDictionary<K, V>, IHashMap<K, V> where K : notnull
    {
        private readonly Dictionary<K, V> _dic;

        public HashMap() : this(0)
        {
        }

        public HashMap(int capacity)
        {
            this._dic = new Dictionary<K, V>(capacity);
        }

        public HashMap(IEnumerable<KeyValuePair<K, V>> src) : this(0)
        {
            this.AddAll(src);
        }

        protected override bool Update(K key, V value)
        {
            if (!this.ContainsKey(key))
            {
                return false;
            }
            this._dic[key] = value;
            return true;
        }

        #region ICollection
        public override int Count => _dic.Count;

        public override bool IsReadOnly => false;

        public override void Clear() => _dic.Clear();
        #endregion

        #region IDictionary
        public override ICollection<K> Keys => _dic.Keys;

        public override ICollection<V> Values => _dic.Values;

        public override void Add(K key, V value) => _dic.Add(key, value);

        public override bool ContainsKey(K key) => _dic.ContainsKey(key);

        public override bool Remove(K key) => _dic.Remove(key);

        public override bool TryGetValue(K key, out V value) => _dic.TryGetValue(key, out value);
        #endregion
    }
}
