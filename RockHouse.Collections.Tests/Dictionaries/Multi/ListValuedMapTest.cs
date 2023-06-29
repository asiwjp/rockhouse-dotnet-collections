using RockHouse.Collections.Dictionaries.Multi;
using System.Collections.Generic;
using System.Text.Json;

namespace Tests.Dictionaries.Multi
{
    public class ListValuedMapTest : AbstractListValuedDictionaryTestBase
    {
        public override IMultiValuedMap<K, V, IList<V>> NewInstance<K, V>() => new ListValuedMap<K, V>();
        public override IMultiValuedMap<K, V, IList<V>> NewInstance<K, V>(int capacity) => new ListValuedMap<K, V>(capacity);
        public override IMultiValuedMap<K, V, IList<V>> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src) => new ListValuedMap<K, V>(src);
        public override IMultiValuedMap<K, V, IList<V>> NewInstance<K, V>(IEqualityComparer<K>? comparer) => new ListValuedMap<K, V>(comparer);
        public override IMultiValuedMap<K, V, IList<V>> NewInstance<K, V>(int capacity, IEqualityComparer<K>? comparer) => new ListValuedMap<K, V>(capacity, comparer);
        public override IMultiValuedMap<K, V, IList<V>> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src, IEqualityComparer<K>? comparer) => new ListValuedMap<K, V>(src, comparer);

        public override IMultiValuedMap<K, V, IList<V>> Deserialize_BySystemTextJson<K, V>(string json)
        {
            return JsonSerializer.Deserialize<ListValuedMap<K, V>>(json);
        }

        public override string Serialize_BySystemTextJson<K, V>(IMultiValuedMap<K, V, IList<V>> dic)
        {
            return JsonSerializer.Serialize(dic as ListValuedMap<K, V>);
        }
    }
}
