using RockHouse.Collections.Dictionaries;
using System.Collections.Generic;
using System.Text.Json;

namespace Tests.Dictionaries
{
    public class LruMapTest : AbstractLruDictionaryTestBase
    {
        public override IHashMap<K, V> NewInstance<K, V>()
        {
            return new LruMap<K, V>();
        }

        public override IHashMap<string, int> NewInstance(int capacity)
        {
            return new LruMap<string, int>(capacity);
        }

        public override IHashMap<string, int> NewInstance(IEnumerable<KeyValuePair<string, int>> src)
        {
            return new LruMap<string, int>(src);
        }

        public override LruDictionary<K, V> NewLruInstance<K, V>()
        {
            return new LruMap<K, V>();
        }

        public override LruDictionary<K, V> NewLruInstance<K, V>(int capacity)
        {
            return new LruMap<K, V>(capacity);
        }

        public override IHashMap<K, V> NewInstance<K, V>(IEqualityComparer<K>? comparer)
        {
            return new LruMap<K, V>(comparer);
        }

        public override IHashMap<K, V> NewInstance<K, V>(int capacity, IEqualityComparer<K>? comparer)
        {
            return new LruMap<K, V>(capacity, comparer);
        }

        public override IHashMap<K, V> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src, IEqualityComparer<K>? comparer)
        {
            return new LruMap<K, V>(src, comparer);
        }


        public override IHashMap<K, V> Deserialize_BySystemTextJson<K, V>(string json)
        {
            return JsonSerializer.Deserialize<LruMap<K, V>>(json);
        }

        public override string Serialize_BySystemTextJson<K, V>(IHashMap<K, V> dictionary)
        {
            return JsonSerializer.Serialize(dictionary as LruMap<K, V>);
        }

    }
}