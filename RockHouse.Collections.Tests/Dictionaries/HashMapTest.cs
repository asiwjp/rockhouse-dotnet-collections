using RockHouse.Collections.Dictionaries;
using System.Collections.Generic;
using System.Text.Json;

namespace Tests.Dictionaries
{
    public class HashMapTest : AbstractDictionaryTestBase
    {
        public override IHashMap<K, V> NewInstance<K, V>()
        {
            return new HashMap<K, V>();
        }

        public override IHashMap<string, int> NewInstance(int capacity)
        {
            return new HashMap<string, int>(capacity);
        }

        public override IHashMap<string, int> NewInstance(IEnumerable<KeyValuePair<string, int>> dictionary)
        {
            return new HashMap<string, int>(dictionary);
        }

        public override IHashMap<K, V> NewInstance<K, V>(IEqualityComparer<K>? comparer)
        {
            return new HashMap<K, V>(comparer);
        }

        public override IHashMap<K, V> NewInstance<K, V>(int capacity, IEqualityComparer<K>? comparer)
        {
            return new HashMap<K, V>(capacity, comparer);
        }

        public override IHashMap<K, V> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src, IEqualityComparer<K>? comparer)
        {
            return new HashMap<K, V>(src, comparer);
        }

        public override IHashMap<K, V> Deserialize_BySystemTextJson<K, V>(string json)
        {
            return JsonSerializer.Deserialize<HashMap<K, V>>(json);
        }

        public override string Serialize_BySystemTextJson<K, V>(IHashMap<K, V> dictionary)
        {
            return JsonSerializer.Serialize(dictionary as HashMap<K, V>);
        }
    }
}