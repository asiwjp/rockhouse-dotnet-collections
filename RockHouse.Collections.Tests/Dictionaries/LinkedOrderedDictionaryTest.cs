using RockHouse.Collections.Dictionaries;
using System.Collections.Generic;
using System.Text.Json;

namespace RockHouse.Collections.Tests.Dictionaries
{
    public class LinkedOrderedDictionary : AbstractOrderedDictionaryTestBase
    {
        public override IHashMap<K, V> NewInstance<K, V>()
        {
            return new LinkedOrderedDictionary<K, V>();
        }

        public override IHashMap<string, int> NewInstance(int capacity)
        {
            return new LinkedOrderedDictionary<string, int>(capacity);
        }

        public override IHashMap<string, int> NewInstance(IEnumerable<KeyValuePair<string, int>> dictionary)
        {
            return new LinkedOrderedDictionary<string, int>(dictionary);
        }

        public override IHashMap<K, V> Deserialize_BySystemTextJson<K, V>(string json)
        {
            return JsonSerializer.Deserialize<LinkedOrderedDictionary<K, V>>(json);
        }

        public override string Serialize_BySystemTextJson<K, V>(IHashMap<K, V> dictionary)
        {
            return JsonSerializer.Serialize(dictionary as LinkedOrderedDictionary<K, V>);
        }
    }
}