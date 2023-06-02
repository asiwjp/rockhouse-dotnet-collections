using RockHouse.Collections.Dictionaries.Multi;
using RockHouse.Collections.Sets;
using System.Collections.Generic;
using System.Text.Json;

namespace RockHouse.Collections.Tests.Dictionaries.Multi
{
    public class HashSetValuedDictionaryTest : AbstractSetValuedDictionaryTestBase
    {
        public override IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>() => new HashSetValuedDictionary<K, V>();
        public override IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>(int capacity) => new HashSetValuedDictionary<K, V>(capacity);
        public override IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src) => new HashSetValuedDictionary<K, V>(src);

        public override IMultiValuedMap<K, V, ISet<V>> Deserialize_BySystemTextJson<K, V>(string json)
        {
            return JsonSerializer.Deserialize<HashSetValuedDictionary<K, V>>(json);
        }

        public override string Serialize_BySystemTextJson<K, V>(IMultiValuedMap<K, V, ISet<V>> dic)
        {
            return JsonSerializer.Serialize(dic as HashSetValuedDictionary<K, V>);
        }
    }
}
