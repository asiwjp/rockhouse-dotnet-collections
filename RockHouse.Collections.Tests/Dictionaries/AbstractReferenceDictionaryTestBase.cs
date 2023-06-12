using RockHouse.Collections.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RockHouse.Collections.Tests.Dictionaries
{
    public abstract class AbstractReferenceDictionaryTestBase : TestBase
    {
        public abstract IHashMap<K, V> NewInstance<K, V>();

        public abstract IHashMap<K, V> NewInstance<K, V>(int capacity);

        public abstract IHashMap<K, V> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src);

        public abstract string Serialize_BySystemTextJson<K, V>(IHashMap<K, V> dictionary);

        public abstract IHashMap<K, V> Deserialize_BySystemTextJson<K, V>(string json);

        internal IDictionary<int, LinkedList<ReferenceEntry<K, V>>> GetInternalDic<K, V>(IHashMap<K, V> o)
        {
            return GetFieldValue<IDictionary<int, LinkedList<ReferenceEntry<K, V>>>>(o, "_dic");
        }

        // NV=Not Volatile
        protected readonly string NV_A = "a";
        protected readonly string NV_B = "b";
        protected readonly Tuple<string> NV_TPL_A = Tuple.Create("a");
        protected readonly Tuple<string> NV_TPL_B = Tuple.Create("b");

        [Fact]
        public void Test___ctor()
        {
            var col = NewInstance<string, Tuple<string>>();
            Assert.Empty(col);
        }

        [Fact]
        public void Test___ctor_with_capacity()
        {
            var col = NewInstance<string, Tuple<string>>(1);
            Assert.Empty(col);
            // If does not thorw exception, OK.
        }

        [Fact]
        public void Test___ctor_with_enumerable()
        {
            var src = new List<KeyValuePair<string, Tuple<string>>>
            {
                new KeyValuePair<string, Tuple < string >>("b", NV_TPL_B ),
                new KeyValuePair<string, Tuple < string >>("a", NV_TPL_A ),
            };
            var col = NewInstance<string, Tuple<string>>(src);

            Assert.Equal(2, col.Count);
            Assert.Equal(new string[] { "b", "a" }, col.Keys.ToArray());
            Assert.Equal(new Tuple<string>[] { NV_TPL_B, NV_TPL_A }, col.Values.ToArray());
        }

        [Fact]
        public void Test___ctor_with_enumerable_if_duplicate_keys()
        {
            Assert.Throws<ArgumentException>(() => NewInstance(new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("b", 11 ),
                new KeyValuePair<string, int>("b", 11 ),
            }));
        }

        [Fact]
        public void Test___indexer()
        {
            var col = NewInstance<string, Tuple<string>>();
            col.Add(NV_A, NV_TPL_A);
            col[NV_A] = NV_TPL_B;

            Assert.Equal(NV_TPL_B, col[NV_A]);
        }

        [Fact]
        public void Test___indexer_if_notfound()
        {
            var col = NewInstance<string, Tuple<string>>();
            col[NV_A] = NV_TPL_A;

            Assert.Equal(NV_TPL_A, col[NV_A]);
            Assert.Throws<KeyNotFoundException>(() => col["notfound"]);
        }

        [Fact]
        public void Test___indexer_update_if_hashKey_conflict()
        {
            var conflict1 = new HashConflictObj(1, "a");
            var conflict2 = new HashConflictObj(1, "b");

            var col = NewInstance<HashConflictObj, Tuple<string>>();
            col.Add(conflict1, NV_TPL_A);

            col[conflict1] = NV_TPL_A;
            col[conflict2] = NV_TPL_B;

            Assert.Equal(NV_TPL_A, col[conflict1]);
            Assert.Equal(NV_TPL_B, col[conflict2]);
        }


        [Fact]
        public void Test__json_Deserialize()
        {
            Assert.Throws<InvalidOperationException>(() => this.Deserialize_BySystemTextJson<string, string>(""));
        }

        [Fact]
        public void Test__json_Serialize()
        {
            var col = NewInstance<string, string>();
            Assert.Throws<InvalidOperationException>(() => this.Serialize_BySystemTextJson(col));
        }

        [Fact]
        public void Test__ICollection__prop_IsReadOnly()
        {
            var col = NewInstance<string, object>();
            Assert.False(col.IsReadOnly);
        }

        [Fact]
        public void Test__ICollection_Clear()
        {
            var col = NewInstance<string, object>();
            col.Add(NV_A, NV_A);
            Assert.True(col.ContainsKey(NV_A));

            col.Clear();
            Assert.False(col.ContainsKey(NV_A));
        }


        [Fact]
        public void Test_Add_canAdd()
        {
            var col = NewInstance<string, Tuple<string>>();
            col.Add(NV_B, NV_TPL_B);
            col.Add(NV_A, NV_TPL_A);

            Assert.Equal(2, col.Count);
            Assert.Equal(NV_TPL_A, col[NV_A]);
            Assert.Equal(NV_TPL_B, col[NV_B]);
        }

        [Fact]
        public void Test_Add_if_already_exists()
        {
            var col = NewInstance<string, int>();
            col.Add(NV_A, 11);

            Assert.Throws<ArgumentException>(() => col.Add(NV_A, 99));
        }

        [Fact]
        public void Test_ContainsKey()
        {
            var col = NewInstance<string, object>();
            col.Add(NV_A, NV_A);
            Assert.True(col.ContainsKey(NV_A));
        }

        [Fact]
        public void Test_ContainsKey_if_notFound()
        {
            var col = NewInstance<string, object>();
            Assert.False(col.ContainsKey(NV_A));
        }

        [Fact]
        public void Test_Remove_canRemove()
        {
            const int hash = 1;

            var hashConflict1 = new HashConflictObj(hash, "a");
            var hashConflict2 = new HashConflictObj(hash, "b");

            var col = NewInstance<HashConflictObj, int>();
            col.Add(hashConflict1, 1000);
            col.Add(hashConflict2, 1001);

            col.Remove(hashConflict2);
            Assert.Single(col);

            col.Remove(hashConflict1);
            Assert.Empty(col);
        }

        [Fact]
        public void Test_Remove_if_empty()
        {
            var col = NewInstance<string, int>();

            col.Remove("notfound");
            Assert.Empty(col);
        }

        [Fact]
        public void Test_Remove_if_notfound()
        {
            var col = this.NewInstance<string, int>();
            col.Add(NV_A, 11);

            col.Remove("notfound");
            Assert.Single(col);
            Assert.Contains<string>(NV_A, col.Keys);
        }


        [Fact]
        public void Test_TryGetValue_canGet()
        {
            var col = this.NewInstance<string, int>();
            col.Add("a", 11);

            var actualRet = col.TryGetValue("a", out int actualValue);
            Assert.True(actualRet);
            Assert.Equal(11, actualValue);
        }

        [Fact]
        public void Test_TryGetValue_if_empty()
        {
            var col = NewInstance<string, int>();

            var actualRet = col.TryGetValue("notfound", out var actualValue);
            Assert.False(actualRet);
            Assert.Equal(default, actualValue);
        }

        [Fact]
        public void Test_TryGetValue_if_notfound()
        {
            var col = this.NewInstance<string, int>();
            col.Add("a", 11);

            var actualRet = col.TryGetValue("notfound", out var actualValue);
            Assert.False(actualRet);
            Assert.Equal(default, actualValue);
        }

        public class HashConflictObj
        {
            public HashConflictObj(int hashCode, string id)
            {
                ID = id;
                HashCode = hashCode;
            }

            public string ID { get; set; }
            public int HashCode { get; set; }

            public override int GetHashCode()
            {
                return this.HashCode;
            }
        }
    }
}
