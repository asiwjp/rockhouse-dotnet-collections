using RockHouse.Collections.Dictionaries.Multi;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RockHouse.Collections.Tests.Dictionaries.Multi
{
    public abstract class AbstractListValuedDictionaryTestBase
    {
        public abstract IMultiValuedMap<K, V, IList<V>> NewInstance<K, V>();
        public abstract IMultiValuedMap<K, V, IList<V>> NewInstance<K, V>(int capacity);
        public abstract IMultiValuedMap<K, V, IList<V>> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src);
        public abstract IMultiValuedMap<K, V, IList<V>> Deserialize_BySystemTextJson<K, V>(string json);
        public abstract string Serialize_BySystemTextJson<K, V>(IMultiValuedMap<K, V, IList<V>> dic);


        [Fact]
        public void Test___ctor()
        {
            var col = NewInstance<string, int>();
            Assert.Empty(col);
        }

        [Fact]
        public void Test___ctor_with_capacity()
        {
            NewInstance<string, int>(10);
            // If does not thorw exception, OK.
        }

        [Fact]
        public void Test___ctor_with_enumerable()
        {
            var col = NewInstance(new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("b", 11 ),
                new KeyValuePair<string, int>("a", 10 ),
            });

            Assert.Equal(2, col.Count);
            Assert.Equal(new string[] { "b", "a" }, col.Keys.ToArray());
            Assert.Equal(new int[] { 11, 10 }, col.Values.ToArray());
        }

        [Fact]
        public void Test___ctor_with_enumerable_if_duplicate_keys()
        {
            var col = NewInstance(new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("b", 11 ),
                new KeyValuePair<string, int>("b", 11 ),
            });

            Assert.Equal(2, col.Count);
            Assert.Equal(new string[] { "b" }, col.Keys.ToArray());
            Assert.Equal(new int[] { 11, 11 }, col.Values.ToArray());
            Assert.Equal(new int[] { 11, 11 }, col["b"]);
        }


        [Fact]
        public void Test___indexer()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 99);
            col["a"] = new int[] { 1 };

            Assert.Equal(new int[] { 1 }, col["a"]);
        }

        [Fact]
        public void Test___indexer_after_removes_and_adds()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 1);

            var values = col["a"];
            Assert.Single(values);

            values.Clear();
            Assert.Empty(col["a"]);
            Assert.False(col.ContainsKey("a"));

            values.Add(1);
            Assert.Single(values);
            Assert.True(col.ContainsKey("a"));

            values.Remove(1);
            Assert.Empty(col["a"]);
            Assert.False(col.ContainsKey("a"));

            col["a"] = new int[] { 100, 101 };
            Assert.Equal(2, values.Count);
            Assert.Equal(100, values[0]);
            Assert.Equal(101, values[1]);

            values[1] = 201;
            Assert.Equal(new int[] { 100, 201 }, col["a"]);

        }

        [Fact]
        public void Test___indexer_if_notfound()
        {
            var col = NewInstance<string, int>();

            Assert.Empty(col["notfound"]);
        }

        [Fact]
        public void Test___withStruct()
        {
            var col = NewInstance<TestStruct, TestStruct>();
            col.Add(new TestStruct(1), new TestStruct(11));
            col[new TestStruct(1)] = new TestStruct[] { new TestStruct(12) };
            col[new TestStruct(2)] = new TestStruct[] { new TestStruct(13) };

            Assert.Equal(new TestStruct[] { new TestStruct(12) }, col[new TestStruct(1)]);
            Assert.Equal(new TestStruct[] { new TestStruct(13) }, col[new TestStruct(2)]);

            Assert.True(col.Remove(new TestStruct(1)));
            Assert.Single(col);
        }

        [Fact]
        public void Test__ICollection__prop_IsReadOnly()
        {
            var col = NewInstance<string, int>();
            Assert.False(col.IsReadOnly);
        }

        [Fact]
        public void Test__ICollection_Clear()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 11);
            col.Add("a", 12);
            col.Add("b", 13);

            col.Clear();
            Assert.Empty(col);
        }

        [Theory]
        [InlineData(true, "a", 1)]
        [InlineData(false, "a", 0)]
        [InlineData(false, "b", 1)]
        [InlineData(false, "b", 0)]
        public void Test__ICollection_Contains(bool expected, string key, int value)
        {
            var col = NewInstance<string, int>();
            col.Add("a", 2);
            col.Add("a", 1);

            var actual = col.Contains(new KeyValuePair<string, int>(key, value));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test__ICollection_CopyTo()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 1);
            col.Add("a", 2);
            col.Add("b", 3);

            var actual = new KeyValuePair<string, int>[4];
            col.CopyTo(actual, 1);

            Assert.Null(actual[0].Key);
            Assert.Equal(0, actual[0].Value);
            Assert.Contains(new KeyValuePair<string, int>("a", 1), actual);
            Assert.Contains(new KeyValuePair<string, int>("a", 2), actual);
            Assert.Contains(new KeyValuePair<string, int>("b", 3), actual);
        }

        [Fact]
        public void Test__ICollection_CopyTo_if_index_out_of_range_at_array()
        {
            var col = NewInstance<string, int>();

            var actual = new KeyValuePair<string, int>[1];
            Assert.Throws<ArgumentOutOfRangeException>(() => col.CopyTo(actual, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => col.CopyTo(actual, 2));
        }

        [Fact]
        public void Test__ICollection_CopyTo_if_index_out_of_range_at_this()
        {
            var col = NewInstance<string, int>();

            var actual = new KeyValuePair<string, int>[1];
            col.CopyTo(actual, 0);
            Assert.Null(actual[0].Key);
        }

        [Fact]
        public void Test__ICollection_CopyTo_if_null()
        {
            var col = NewInstance<string, int>();

            Assert.Throws<ArgumentNullException>(() => col.CopyTo(null, 2));
        }

        [Fact]
        public void Test__ICollection_CopyTo_if_too_small()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 1);
            col.Add("a", 2);

            var actual = new KeyValuePair<string, int>[2];
            Assert.Throws<ArgumentException>(() => col.CopyTo(actual, 1));
        }


        [Fact]
        public void Test__ICollection_Remove_with_keyValue()
        {
            var col = NewInstance<string, int>();
            col.Add(new KeyValuePair<string, int>("a", 1));
            var actual = col.Remove(new KeyValuePair<string, int>("a", 1));

            Assert.True(actual);
            Assert.Empty(col);

            // Dictionary compatibility
            ICollection<KeyValuePair<string, int>> std = new Dictionary<string, int>
            {
                {  "a", 1 }
            };
            var expected = std.Remove(new KeyValuePair<string, int>("a", 1));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test__ICollection_Remove_with_keyValue_if_notFound_key_unmatch()
        {
            var query = new KeyValuePair<string, int>("b", 1);

            var col = NewInstance<string, int>();
            col.Add(new KeyValuePair<string, int>("a", 1));
            var actual = col.Remove(query);

            Assert.False(actual);
            Assert.Single(col);

            // Dictionary compatibility
            ICollection<KeyValuePair<string, int>> std = new Dictionary<string, int>
            {
                {  "a", 1 }
            };
            var expected = std.Remove(query);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test__ICollection_Remove_with_keyValue_if_notFound_value_unmatch()
        {
            var query = new KeyValuePair<string, int>("a", 2);

            var col = NewInstance<string, int>();
            col.Add(new KeyValuePair<string, int>("a", 1));
            var actual = col.Remove(query);

            Assert.False(actual);
            Assert.Single(col);

            // Dictionary compatibility
            ICollection<KeyValuePair<string, int>> std = new Dictionary<string, int>
            {
                {  "a", 1 }
            };
            var expected = std.Remove(query);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test__IEnumerable_GetEnumerator()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 11);
            col.Add("b", 13);
            col.Add("a", 12);


            var keyValues = col.ToArray();
            keyValues.Contains(new KeyValuePair<string, int>("a", 11));
            keyValues.Contains(new KeyValuePair<string, int>("a", 12));
            keyValues.Contains(new KeyValuePair<string, int>("b", 13));
        }

        [Fact]
        public void Test__IEnumerable_GetEnumerator_if_empty()
        {
            var col = NewInstance<string, int>();
            Assert.Empty(col);
        }

        [Fact]
        public void Test__IMultiValuedMap_Get()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 1);
            col.Add("a", 2);

            Assert.Equal(new int[] { 1, 2 }, col.Get("a"));
        }

        [Fact]
        public void Test__IMultiValuedMap_Get_after_removes_and_adds()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 1);

            col.Get("a").Clear();
            Assert.Empty(col["a"]);
            Assert.False(col.ContainsKey("a"));

            col.Get("a").Add(1);
            Assert.Single(col["a"]);
            Assert.True(col.ContainsKey("a"));

            col.Get("a").Remove(1);
            Assert.Empty(col["a"]);
            Assert.False(col.ContainsKey("a"));
        }

        [Fact]
        public void Test__IMultiValuedMap_Get_if_notfound()
        {
            var col = NewInstance<string, int>();

            Assert.Empty(col.Get("a"));
        }

        [Fact]
        public void Test__IMultiValuedMap_Put()
        {
            var col = NewInstance<string, int>();

            var actual1 = col.Put("a", 1);
            Assert.Equal(new int[] { 1 }, col["a"]);
            Assert.True(actual1);

            var actual2 = col.Put("a", 2);
            Assert.Equal(new int[] { 1, 2 }, col["a"]);
            Assert.True(actual2);

            var actual3 = col.Put("b", 3);
            Assert.Equal(new int[] { 1, 2 }, col["a"]);
            Assert.Equal(new int[] { 3 }, col["b"]);
            Assert.True(actual3);
        }

        [Fact]
        public void Test__Object_ToString()
        {
            var col = NewInstance<string, int>();
            col.Add("", 1);

            Assert.Equal("Count=1", col.ToString());
        }

        [Fact]
        public void Test__json_Deserialize()
        {
            var expected = new KeyValuePair<string, int>[]
            {
                new KeyValuePair<string, int>("b", 1),
                new KeyValuePair<string, int>("a", 2),
                new KeyValuePair<string, int>("a", 3),
            };

            var actualSystem = this.Deserialize_BySystemTextJson<string, int>(@"{""b"":[1],""a"":[2,3]}");
            Assert.Equal(expected, actualSystem.ToArray());
        }

        [Fact]
        public void Test__json_Serialize()
        {
            var expected = @"{""b"":[1],""a"":[2,3]}";

            var col = NewInstance<string, int>();
            col.Add("b", 1);
            col.Add("a", 2);
            col.Add("a", 3);

            var actualSystem = this.Serialize_BySystemTextJson(col);
            Assert.Equal(expected, actualSystem);
        }

        [Fact]
        public void Test__prop_Keys()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 9);
            col.Add("b", 1);
            col["a"] = new int[] { 2, 3 };

            var keys = col.Keys.ToList();
            Assert.Equal(2, keys.Count);
            Assert.Contains("a", keys);
            Assert.Contains("b", keys);

            // is immutable
            Assert.Throws<NotSupportedException>(() => col.Keys.Remove("a"));
        }

        [Fact]
        public void Test__prop_Values()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 9);
            col.Add("b", 1);
            col["a"] = new int[] { 2, 3, 4 };

            var values = col.Values.ToList();
            Assert.Equal(4, values.Count);
            Assert.Contains(1, values);
            Assert.Contains(2, values);
            Assert.Contains(3, values);
            Assert.Contains(4, values);

            // is immutable
            Assert.Throws<NotSupportedException>(() => col.Values.Remove(1));
        }

        [Fact]
        public void Test_Add_canAdd()
        {
            var col = NewInstance<string, int>();
            col.Add("b", 11);
            col.Add("a", 12);
            col.Add("a", 13);

            Assert.Equal(3, col.Count);
            Assert.Contains(new KeyValuePair<string, int>("a", 12), col);
            Assert.Contains(new KeyValuePair<string, int>("a", 13), col);
            Assert.Contains(new KeyValuePair<string, int>("b", 11), col);
        }

        [Fact]
        public void Test_Add_if_already_exists()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 11);
            col.Add("a", 12);
            // OK if no exceptions are thrown.

            Assert.Equal(new int[] { 11, 12 }, col["a"].ToArray());
        }

        [Fact]
        public void Test_Add_keyValuePair()
        {
            var col = NewInstance<string, int>();
            col.Add(new KeyValuePair<string, int>("a", 11));
            col.Add(new KeyValuePair<string, int>("a", 12));
            Assert.Equal(new int[] { 11, 12 }, col["a"].ToArray());
        }

        [Fact]
        public void Test_ContainsKey()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 1);

            var actual = col.ContainsKey("a");
            Assert.True(actual);
        }

        [Fact]
        public void Test_ContainsKey_after_removes_and_adds()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 1);
            Assert.Single(col["a"]);

            col["a"].Clear();
            Assert.False(col.ContainsKey("a"));

            col["a"].Add(2);
            Assert.True(col.ContainsKey("a"));

            col["a"].Remove(2);
            Assert.False(col.ContainsKey("a"));
        }

        [Fact]
        public void Test_ContainsKey_if_notfound()
        {
            var col = NewInstance<string, int>();

            var actual = col.ContainsKey("notfound");

            Assert.False(actual);
        }

        [Fact]
        public void Test_Remove_canRemove()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 11);
            col.Add("a", 12);
            col.Add("b", 13);

            col.Remove("a");
            Assert.Single(col);
            Assert.DoesNotContain<string>("a", col.Keys);

            col.Remove("b");
            Assert.Empty(col);
            Assert.DoesNotContain<string>("b", col.Keys);
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
            col.Add("a", 11);

            col.Remove("notfound");

            Assert.Single(col);
            Assert.Contains<string>("a", col.Keys);
        }

        [Fact]
        public void Test_TryGetValue_canGet()
        {
            var col = this.NewInstance<string, int>();
            col.Add("a", 11);
            col.Add("a", 12);

            var actualRet = col.TryGetValue("a", out var actualValue);
            Assert.True(actualRet);
            Assert.Equal(2, actualValue.Count);
            Assert.Equal(11, actualValue[0]);
            Assert.Equal(12, actualValue[1]);
        }

        [Fact]
        public void Test_TryGetValue_if_empty()
        {
            var col = NewInstance<string, int>();

            var actualRet = col.TryGetValue("notfound", out var actualValue);
            Assert.False(actualRet);
            Assert.Empty(actualValue);
        }

        [Fact]
        public void Test_TryGetValue_if_notfound()
        {
            var col = this.NewInstance<string, int>();
            col.Add("a", 11);

            var actualRet = col.TryGetValue("notfound", out var actualValue);
            Assert.False(actualRet);
            Assert.Empty(actualValue);
        }

        [Fact]
        public void Test_ValuesProxy__prop_IsReadOnly()
        {
            var col = this.NewInstance<string, int>();

            var actual1 = col["a"].IsReadOnly;
            Assert.False(actual1);
        }

        [Fact]
        public void Test_ValuesProxy_Contains()
        {
            var col = this.NewInstance<string, int>();
            col.Add("a", 11);

            var actual1 = col["a"].Contains(11);
            Assert.True(actual1);

            var actual2 = col["a"].Contains(99);
            Assert.False(actual2);
        }

        [Fact]
        public void Test_ValuesProxy_IndexOf()
        {
            var col = this.NewInstance<string, int>();
            col.Add("a", 11);

            var actual1 = col["a"].IndexOf(11);
            Assert.Equal(0, actual1);

            var actual2 = col["a"].IndexOf(99);
            Assert.Equal(-1, actual2);
        }

        [Fact]
        public void Test_ValuesProxy_Insert()
        {
            var col = this.NewInstance<string, int>();
            col.Add("a", 11);

            var proxy = col["a"];
            proxy.Insert(0, 10);
            Assert.Equal(new int[] { 10, 11 }, col["a"]);

            proxy.Clear();
            Assert.False(col.ContainsKey("a"));

            proxy.Insert(0, 12);
            Assert.True(col.ContainsKey("a"));
            Assert.Equal(new int[] { 12 }, col["a"]);
        }

        [Fact]
        public void Test_ValuesProxy_RemoveAt()
        {
            var col = this.NewInstance<string, int>();
            col.Add("a", 11);
            col.Add("a", 12);

            var proxy = col["a"];
            proxy.RemoveAt(1);
            Assert.Equal(new int[] { 11 }, col["a"]);

            proxy.RemoveAt(0);
            Assert.False(col.ContainsKey("a"));
        }

    }
}
