﻿using RockHouse.Collections.Dictionaries.Multi;
using System.Collections.Generic;
using System.Text.Json;

namespace RockHouse.Collections.Tests.Dictionaries.Multi
{
    public class ListValuedDictionaryTest : AbstractListValuedDictionaryTestBase
    {
        public override IMultiValuedMap<K, V, IList<V>> NewInstance<K, V>() => new ListValuedDictionary<K, V>();
        public override IMultiValuedMap<K, V, IList<V>> NewInstance<K, V>(int capacity) => new ListValuedDictionary<K, V>(capacity);
        public override IMultiValuedMap<K, V, IList<V>> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src) => new ListValuedDictionary<K, V>(src);

        public override IMultiValuedMap<K, V, IList<V>> Deserialize_BySystemTextJson<K, V>(string json)
        {
            return JsonSerializer.Deserialize<ListValuedDictionary<K, V>>(json);
        }

        public override string Serialize_BySystemTextJson<K, V>(IMultiValuedMap<K, V, IList<V>> dic)
        {
            return JsonSerializer.Serialize(dic as ListValuedDictionary<K, V>);
        }
    }
}
