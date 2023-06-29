using RockHouse.Collections;
using RockHouse.Collections.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Dictionaries
{
    public class WeakHashMapTest : AbstractReferenceDictionaryTestBase
    {
        public override IHashMap<K, V> NewInstance<K, V>()
        {
            return new WeakHashMap<K, V>();
        }

        public override IHashMap<K, V> NewInstance<K, V>(int capacity)
        {
            return new WeakHashMap<K, V>(capacity);
        }

        public override IHashMap<K, V> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src)
        {
            return new WeakHashMap<K, V>(src);
        }

        public override IHashMap<K, V> NewInstance<K, V>(IEqualityComparer<K>? comparer)
        {
            return new WeakHashMap<K, V>(comparer);
        }

        public override IHashMap<K, V> NewInstance<K, V>(int capacity, IEqualityComparer<K>? comparer)
        {
            return new WeakHashMap<K, V>(capacity, comparer);
        }

        public override IHashMap<K, V> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src, IEqualityComparer<K>? comparer)
        {
            return new WeakHashMap<K, V>(src, comparer);
        }

        public override string Serialize_BySystemTextJson<K, V>(IHashMap<K, V> dictionary)
        {
            return JsonSerializer.Serialize((WeakHashMap<K, V>)dictionary);
        }

        public override IHashMap<K, V> Deserialize_BySystemTextJson<K, V>(string json)
        {
            return JsonSerializer.Deserialize<WeakHashMap<K, V>>(json);
        }

        [Fact]
        public void Test___ctor_refStrength()
        {
            var col1 = new WeakHashMap<string, object>();
            var col2 = new WeakHashMap<string, object>(10);
            var col3 = new WeakHashMap<string, string>(new KeyValuePair<string, string>[] { });

            Assert.Equal(ReferenceStrength.Weak, col1.KeyReferenceStrength);
            Assert.Equal(ReferenceStrength.Weak, col2.KeyReferenceStrength);
            Assert.Equal(ReferenceStrength.Weak, col3.KeyReferenceStrength);
            Assert.Equal(ReferenceStrength.Hard, col1.ValueReferenceStrength);
            Assert.Equal(ReferenceStrength.Hard, col2.ValueReferenceStrength);
            Assert.Equal(ReferenceStrength.Hard, col3.ValueReferenceStrength);
        }

        [Fact]
        public void Test__IEnumerable_GetEnumerator()
        {
            var col = NewInstance<string, string>();
            col.Add(NV_A, NV_B);

            var enumerator = col.GetEnumerator();
            Assert.True(enumerator.MoveNext());
            Assert.Equal(NV_A, enumerator.Current.Key);
            Assert.Equal(NV_B, enumerator.Current.Value);
        }

        [Fact]
        public void Test__IEnumerable_GetEnumerator_with_gc()
        {
            var col = NewInstance<Tuple<string>, object>();
            Task.Run(() =>
            {
                col.Add(Tuple.Create(""), NV_A);
            }).Wait();
            ForceGC();

            Assert.False(col.GetEnumerator().MoveNext());
        }

        [Fact]
        public void Test_Add_with_gc()
        {
            var col = NewInstance<Tuple<string>, object>();
            Task.Run(() =>
            {
                col.Add(Tuple.Create("x"), new object());
            }).Wait();
            ForceGC();

            var tmp = col.Keys.ToList(); // dummy scan and purge
            var key = Tuple.Create("x");
            col.Add(key, NV_A);
            Assert.True(col.ContainsKey(key));
        }

        [Fact]
        public void Test_ContainsKey_with_gc()
        {
            var col = NewInstance<Tuple<string>, object>();
            Task.Run(() =>
            {
                col.Add(Tuple.Create("x"), new object());
            }).Wait();
            ForceGC();

            Assert.False(col.ContainsKey(Tuple.Create("x")));
        }

        [Fact]
        public void Test_TryGetValue_with_gc()
        {
            var col = this.NewInstance<Tuple<string>, object>();
            Task.Run(() =>
            {
                col.Add(Tuple.Create("x"), new object());

            }).Wait();
            ForceGC();

            var actualRet = col.TryGetValue(Tuple.Create("x"), out var actualValue);
            Assert.False(actualRet);
            Assert.Equal(default, actualValue);
            Assert.Empty(col);
        }
    }
}
