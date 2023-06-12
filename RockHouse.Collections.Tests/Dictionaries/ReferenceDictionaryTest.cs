using RockHouse.Collections.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace RockHouse.Collections.Tests.Dictionaries
{
    public class ReferenceDictionaryTest : AbstractReferenceDictionaryTestBase
    {
        public override IHashMap<K, V> NewInstance<K, V>()
        {
            return new ReferenceDictionary<K, V>();
        }

        public override IHashMap<K, V> NewInstance<K, V>(int capacity)
        {
            return new ReferenceDictionary<K, V>(capacity);
        }

        public override IHashMap<K, V> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src)
        {
            return new ReferenceDictionary<K, V>(src);
        }

        public override string Serialize_BySystemTextJson<K, V>(IHashMap<K, V> dictionary)
        {
            return JsonSerializer.Serialize((ReferenceDictionary<K, V>)dictionary);
        }

        public override IHashMap<K, V> Deserialize_BySystemTextJson<K, V>(string json)
        {
            return JsonSerializer.Deserialize<ReferenceDictionary<K, V>>(json);
        }

        [Fact]
        public void Test___ctor_refStrength()
        {
            var col1 = new ReferenceDictionary<string, object>();
            var col2 = new ReferenceDictionary<string, object>(10);
            var col3 = new ReferenceDictionary<string, object>(ReferenceStrength.Weak, ReferenceStrength.Hard, 0);
            var col4 = new ReferenceDictionary<string, string>(new KeyValuePair<string, string>[] { });

            Assert.Equal(ReferenceStrength.Hard, col1.KeyReferenceStrength);
            Assert.Equal(ReferenceStrength.Weak, col1.ValueReferenceStrength);

            Assert.Equal(ReferenceStrength.Hard, col2.KeyReferenceStrength);
            Assert.Equal(ReferenceStrength.Weak, col2.ValueReferenceStrength);

            Assert.Equal(ReferenceStrength.Weak, col3.KeyReferenceStrength);
            Assert.Equal(ReferenceStrength.Hard, col3.ValueReferenceStrength);

            Assert.Equal(ReferenceStrength.Hard, col4.KeyReferenceStrength);
            Assert.Equal(ReferenceStrength.Weak, col4.ValueReferenceStrength);
        }

        [Fact]
        public void Test___refStrength_keyHard_valWeak()
        {
            var col = new ReferenceDictionary<string, object>(ReferenceStrength.Hard, ReferenceStrength.Weak, 0);
            Task.Run(() =>
            {
                col.Add(NV_A, new object());
                col.Add(NV_B, NV_B);
            }).Wait();
            this.ForceGC();

            Assert.Single(col);
            Assert.True(col.ContainsKey(NV_B));

            col.Add(NV_A, NV_A);
            Assert.Equal(2, col.Count);
        }

        [Fact]
        public void Test___refStrength_keyWeak_valHard()
        {
            var col = new ReferenceDictionary<object, object>(ReferenceStrength.Weak, ReferenceStrength.Hard, 0);
            Task.Run(() =>
            {
                col.Add(NV_TPL_A, NV_A);
                col.Add(Tuple.Create("x"), NV_A);
            }).Wait();
            this.ForceGC();

            Assert.Single(col);
            Assert.True(col.ContainsKey(NV_TPL_A));

            col.Add(Tuple.Create("x"), NV_A);
            Assert.Equal(2, col.Count);
        }

        [Fact]
        public void Test___indexer_with_gc_if_key_weakref()
        {
            var col = new ReferenceDictionary<Tuple<string>, int>(ReferenceStrength.Weak, ReferenceStrength.Weak, 0);
            Task.Run(() =>
            {
                col[Tuple.Create("a")] = 1;
            }).Wait();
            ForceGC();

            Assert.Throws<KeyNotFoundException>(() => col[Tuple.Create("a")]);
        }


        [Fact]
        public void Test___indexer_with_gc_if_val_weakref()
        {
            var col = new ReferenceDictionary<int, Tuple<string>>();
            Task.Run(() =>
            {
                col[1] = Tuple.Create("x");
            }).Wait();
            ForceGC();

            Assert.Throws<KeyNotFoundException>(() => col[1]);
        }

        [Fact]
        public void Test__IEnumerable_GetEnumerator()
        {
            var col = NewInstance<string, object>();
            col.Add(NV_A, NV_B);

            var enumerator = col.GetEnumerator();
            Assert.True(enumerator.MoveNext());
            Assert.Equal(NV_A, enumerator.Current.Key);
            Assert.Equal(NV_B, enumerator.Current.Value);
        }

        [Fact]
        public void Test__IEnumerable_GetEnumerator_with_gc()
        {
            var col = NewInstance<string, object>();
            Task.Run(() =>
            {
                col.Add(NV_A, new object());
            }).Wait();
            ForceGC();

            Assert.False(col.GetEnumerator().MoveNext());
        }

        [Fact]
        public void Test_Add_with_gc()
        {
            var col = NewInstance<string, object>();
            Task.Run(() =>
            {
                col.Add(NV_A, new object());
            }).Wait();
            ForceGC();

            var tmp = col.Keys.ToList(); // dummy scan and purge
            col.Add(NV_A, NV_A);
            Assert.True(col.ContainsKey(NV_A));
        }

        [Fact]
        public void Test_Add_with_gc_if_key_weakref()
        {
            var col = new ReferenceDictionary<Tuple<string>, string>(ReferenceStrength.Weak, ReferenceStrength.Weak, 0);
            Task.Run(() =>
            {
                col.Add(Tuple.Create("a"), "xxx");
            }).Wait();
            ForceGC();

            var exceptKey = Tuple.Create("a");
            col.Add(exceptKey, NV_A);
            Assert.True(col.ContainsKey(exceptKey));
        }

        [Fact]
        public void Test_ContainsKey_with_gc()
        {
            var col = NewInstance<string, object>();
            Task.Run(() =>
            {
                col.Add(NV_A, new object());
            }).Wait();
            ForceGC();

            Assert.False(col.ContainsKey(NV_A));
        }

        [Fact]
        public void Test_Remove_canRemove_check_strict()
        {
            const int hash = 1;

            var hashConflict1 = new HashConflictObj(hash, "a");
            var hashConflict2 = new HashConflictObj(hash, "b");

            var col = NewInstance<HashConflictObj, string>();
            col.Add(hashConflict1, NV_A);
            col.Add(hashConflict2, NV_B);

            var _dic = this.GetInternalDic<HashConflictObj, string>(col);
            Assert.Equal(2, _dic[1].Count);

            col.Remove(hashConflict2);
            Assert.Single(_dic[hash]);
            Assert.Single(col);

            col.Remove(hashConflict1);
            Assert.False(_dic.ContainsKey(hash));
            Assert.Empty(col);
        }

        [Fact]
        public void Test_Remove_with_gc_if_key_weakref()
        {
            var col = new ReferenceDictionary<HashConflictObj, object>(ReferenceStrength.Weak, ReferenceStrength.Weak, 0);
            Task.Run(() =>
            {
                col.Add(new HashConflictObj(1, "a"), new object());
            }).Wait();
            ForceGC();

            var actual = col.Remove(new HashConflictObj(1, "a"));
            Assert.False(actual);
            Assert.Empty(col);
        }

        [Fact]
        public void Test_TryGetValue_with_gc()
        {
            var col = this.NewInstance<string, object>();
            Task.Run(() =>
            {
                col.Add(NV_A, new object());

            }).Wait();
            ForceGC();

            var actualRet = col.TryGetValue(NV_A, out var actualValue);
            Assert.False(actualRet);
            Assert.Equal(default, actualValue);
            Assert.Empty(col);
        }

        [Fact]
        public void Test_TryGetValue_with_gc_if_key_weakref()
        {
            var col = new ReferenceDictionary<Tuple<string>, object>(ReferenceStrength.Weak, ReferenceStrength.Weak, 0);
            Task.Run(() =>
            {
                col.Add(Tuple.Create("a"), new object());

            }).Wait();
            ForceGC();

            var expectKey = Tuple.Create("a");
            var actualRet = col.TryGetValue(expectKey, out var actualValue);
            Assert.False(actualRet);
            Assert.Equal(default, actualValue);
            Assert.Empty(col);
        }
    }
}
