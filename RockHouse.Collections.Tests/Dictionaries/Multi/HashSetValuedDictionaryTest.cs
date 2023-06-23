using RockHouse.Collections.Dictionaries.Multi;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace RockHouse.Collections.Tests.Dictionaries.Multi
{
    public class HashSetValuedDictionaryTest : AbstractSetValuedDictionaryTestBase
    {
        public override IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>() => new HashSetValuedDictionary<K, V>();
        public override IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>(int capacity) => new HashSetValuedDictionary<K, V>(capacity);
        public override IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src) => new HashSetValuedDictionary<K, V>(src);
        public override IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>(IEqualityComparer<K>? comparer) => new HashSetValuedDictionary<K, V>(comparer, null);
        public override IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>(int capacity, IEqualityComparer<K>? comparer) => new HashSetValuedDictionary<K, V>(capacity, comparer, null);
        public override IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src, IEqualityComparer<K>? comparer) => new HashSetValuedDictionary<K, V>(src, comparer, null);

        public override IMultiValuedMap<K, V, ISet<V>> Deserialize_BySystemTextJson<K, V>(string json)
        {
            return JsonSerializer.Deserialize<HashSetValuedDictionary<K, V>>(json);
        }

        public override string Serialize_BySystemTextJson<K, V>(IMultiValuedMap<K, V, ISet<V>> dic)
        {
            return JsonSerializer.Serialize(dic as HashSetValuedDictionary<K, V>);
        }

        [Fact]
        public void Test___ctor_with_equalityComparer_forValue()
        {
            var col1 = new HashSetValuedDictionary<string, string>(null, new IgnoreCaseStringComparer());
            col1["key"].Add("a");
            col1["key"].Add("A");
            Assert.Single(col1);
            Assert.Equal("a", col1["key"].First());

            var col2 = new HashSetValuedDictionary<string, string>(5, null, new IgnoreCaseStringComparer());
            col2["key"].Add("b");
            col2["key"].Add("B");
            Assert.Single(col2);
            Assert.Equal("b", col2["key"].First());

            var col3 = new HashSetValuedDictionary<string, string>(5, null, new IgnoreCaseStringComparer());
            col3["key"].Add("c");
            col3["key"].Add("C");
            Assert.Single(col3);
            Assert.Equal("c", col3["key"].First());
        }
    }
}
