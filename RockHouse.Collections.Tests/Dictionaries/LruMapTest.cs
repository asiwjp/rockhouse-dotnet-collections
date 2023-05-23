using RockHouse.Collections.Dictionaries;
using System.Collections.Generic;
using System.Text.Json;

namespace RockHouse.Collections.Tests.Dictionaries
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