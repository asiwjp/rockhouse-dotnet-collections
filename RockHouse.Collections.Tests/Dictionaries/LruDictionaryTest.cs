using RockHouse.Collections.Dictionaries;
using System.Collections.Generic;
using System.Text.Json;

namespace RockHouse.Collections.Tests.Dictionaries
{
    public class LruDictionaryTest : AbstractLruDictionaryTestBase
    {
        public override IHashMap<K, V> NewInstance<K, V>()
        {
            return new LruDictionary<K, V>();
        }

        public override IHashMap<string, int> NewInstance(int capacity)
        {
            return new LruDictionary<string, int>(capacity);
        }

        public override IHashMap<string, int> NewInstance(IEnumerable<KeyValuePair<string, int>> src)
        {
            return new LruDictionary<string, int>(src);
        }

        public override LruDictionary<K, V> NewLruInstance<K, V>()
        {
            return new LruDictionary<K, V>();
        }

        public override LruDictionary<K, V> NewLruInstance<K, V>(int capacity)
        {
            return new LruDictionary<K, V>(capacity);
        }

        public override IHashMap<K, V> Deserialize_BySystemTextJson<K, V>(string json)
        {
            return JsonSerializer.Deserialize<LruDictionary<K, V>>(json);
        }

        public override string Serialize_BySystemTextJson<K, V>(IHashMap<K, V> dictionary)
        {
            return JsonSerializer.Serialize(dictionary as LruDictionary<K, V>);
        }
    }
}