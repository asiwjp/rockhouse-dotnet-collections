﻿using RockHouse.Collections.Dictionaries.Multi;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RockHouse.Collections.Tests.Dictionaries.Multi
{
    public abstract class AbstractSetValuedDictionaryTestBase
    {
        public abstract IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>();
        public abstract IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>(int capacity);
        public abstract IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src);
        public abstract IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>(IEqualityComparer<K>? comparer);
        public abstract IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>(int capacity, IEqualityComparer<K>? comparer);
        public abstract IMultiValuedMap<K, V, ISet<V>> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src, IEqualityComparer<K>? comparer);
        public abstract IMultiValuedMap<K, V, ISet<V>> Deserialize_BySystemTextJson<K, V>(string json);
        public abstract string Serialize_BySystemTextJson<K, V>(IMultiValuedMap<K, V, ISet<V>> dic);


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
        public void Test___ctor_with_enumerable_if_duplicate_keyValues()
        {
            var col = NewInstance(new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("b", 12 ),
                new KeyValuePair<string, int>("b", 11 ),
                new KeyValuePair<string, int>("b", 11 ),
            });

            Assert.Equal(2, col.Count);
            Assert.Equal(new string[] { "b" }, col.Keys.ToArray());
            Assert.Equal(new int[] { 12, 11 }, col.Values.ToArray());
            Assert.Equal(new int[] { 12, 11 }, col["b"]);
        }

        [Fact]
        public void Test___ctor_with_equalityComparer()
        {
            var col1 = NewInstance<string, int>(new IgnoreCaseStringComparer());
            col1["a"].Add(11);
            col1["A"].Add(12);
            Assert.Equal(new int[] { 11, 12 }, col1["A"].ToArray());
            col1.Remove("A");
            Assert.Empty(col1);

            var col2 = NewInstance<string, int>(5, new IgnoreCaseStringComparer());
            col2["a"].Add(21);
            col2["A"].Add(22);
            Assert.Equal(new int[] { 21, 22 }, col2["A"].ToArray());
            col2.Remove("A");
            Assert.Empty(col2);

            var col3 = NewInstance<string, int>(new Dictionary<string, int>
            {
                { "a", 31 },
                { "A", 32 },
            },
            new IgnoreCaseStringComparer());
            Assert.Equal(new int[] { 31, 32 }, col3["A"].ToArray());
            col3.Remove("A");
            Assert.Empty(col3);
        }

        [Fact]
        public void Test___indexer()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 1);

            var actual = col["a"];
            Assert.IsAssignableFrom<ISet<int>>(actual);
            Assert.Equal(new int[] { 1 }, actual);

            col["a"] = new HashSet<int> { 2 };
            Assert.IsAssignableFrom<ISet<int>>(actual);
            Assert.Equal(new int[] { 2 }, actual);
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

            col["a"] = new HashSet<int> { 101, 100 };
            Assert.Equal(2, values.Count);
            Assert.Contains(100, values);
            Assert.Contains(101, values);
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
            col[new TestStruct(1)] = new HashSet<TestStruct> { new TestStruct(12) };
            col[new TestStruct(2)] = new HashSet<TestStruct> { new TestStruct(13) };

            Assert.Equal(new TestStruct[] { new TestStruct(12) }, col[new TestStruct(1)]);
            Assert.Equal(new TestStruct[] { new TestStruct(13) }, col[new TestStruct(2)]);

            Assert.True(col.Remove(new TestStruct(1)));
            Assert.Single(col);
        }

        [Fact]
        public void Test__prop_IsEmpty()
        {
            var col = NewInstance<string, int>();
            Assert.True(col.IsEmpty);

            col.Add("a", 1);
            Assert.False(col.IsEmpty);
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
        public void Test__IMultiValuedMap_AddAll()
        {
            var col = NewInstance<string, int>();
            col["a"].Add(1);

            var actual1 = col.AddAll("a", new int[] { 2 });
            Assert.Equal(new int[] { 1, 2 }, col["a"]);
            Assert.True(actual1);
            Assert.Equal(2, col.Count);
        }

        [Fact]
        public void Test__IMultiValuedMap_AddAll_if_duplicateValues()
        {
            var col = NewInstance<string, int>();
            col["a"].Add(1);

            var actual1 = col.AddAll("a", new int[] { 1, 1, 2 });
            Assert.Equal(new int[] { 1, 2 }, col["a"]);
        }

        [Fact]
        public void Test__IMultiValuedMap_AddAll_if_emptySrc()
        {
            var col = NewInstance<string, int>();

            var actual1 = col.AddAll("a", new int[] { });
            Assert.False(actual1);
            Assert.Equal(0, col.Count);
        }

        [Fact]
        public void Test__IMultiValuedMap_AddAll_with_keyValuePairs()
        {
            var col = NewInstance<string, int>();
            col["a"].Add(1);

            var actual1 = col.AddAll(new KeyValuePair<string, int>[]
            {
                new KeyValuePair<string, int>( "a", 2 ),
                new KeyValuePair<string, int>( "a", 3 )
            });
            Assert.Equal(new int[] { 1, 2, 3 }, col["a"]);
            Assert.True(actual1);
            Assert.Equal(3, col.Count);
        }

        [Fact]
        public void Test__IMultiValuedMap_AddAll_with_keyValuePairs_if_duplicateValues()
        {
            var col = NewInstance<string, int>();
            col["a"].Add(1);

            var actual1 = col.AddAll(new KeyValuePair<string, int>[]
            {
                new KeyValuePair<string, int>( "a", 1 ),
                new KeyValuePair<string, int>( "a", 1 )
            });
            Assert.Equal(new int[] { 1 }, col["a"]);
            Assert.True(actual1);
            Assert.Single(col);
        }

        [Fact]
        public void Test__IMultiValuedMap_Delete()
        {
            var col = NewInstance<string, int>();
            col["a"].Add(1);
            col["a"].Add(2);

            var actual1 = col.Delete("a");
            Assert.Equal(new int[] { 1, 2 }, actual1);
            Assert.False(col.ContainsKey("a"));
            Assert.Empty(col["a"]);
        }

        [Fact]
        public void Test__IMultiValuedMap_Delete_if_notfound()
        {
            var col = NewInstance<string, int>();

            var actual1 = col.Delete("notfound");
            Assert.Empty(actual1);
            Assert.False(col.ContainsKey("a"));
            Assert.Empty(col["a"]);
        }

        [Fact]
        public void Test__IMultiValuedMap_Get()
        {
            var col = NewInstance<string, int>();
            col.Add("a", 1);
            col.Add("a", 2);

            Assert.Equal(new HashSet<int> { 1, 2 }, col.Get("a"));
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
            Assert.Equal(new HashSet<int> { 1 }, col["a"]);
            Assert.True(actual1);

            var actual2 = col.Put("a", 2);
            Assert.Equal(new HashSet<int> { 1, 2 }, col["a"]);
            Assert.True(actual2);

            var actual3 = col.Put("b", 3);
            Assert.Equal(new HashSet<int> { 1, 2 }, col["a"]);
            Assert.Equal(new HashSet<int> { 3 }, col["b"]);
            Assert.True(actual3);
        }

        [Fact]
        public void Test__IMultiValuedMap_PutAll()
        {
            var col = NewInstance<string, int>();
            col["a"].Add(1);

            var actual1 = col.PutAll("a", new int[] { 2, 3 });
            Assert.Equal(new int[] { 1, 2, 3 }, col["a"]);
            Assert.True(actual1);
            Assert.Equal(3, col.Count);
        }

        [Fact]
        public void Test__IMultiValuedMap_PutAll_if_duplicateValues()
        {
            var col = NewInstance<string, int>();
            col["a"].Add(1);

            var actual1 = col.PutAll("a", new int[] { 1, 1, 2 });
            Assert.Equal(new int[] { 1, 2 }, col["a"]);
        }

        [Fact]
        public void Test__IMultiValuedMap_PutAll_with_keyValuePairs()
        {
            var col = NewInstance<string, int>();
            col["a"].Add(1);

            var actual1 = col.PutAll(new KeyValuePair<string, int>[]
            {
                new KeyValuePair<string, int>( "a", 2 ),
                new KeyValuePair<string, int>( "a", 3 )
            });
            Assert.Equal(new int[] { 1, 2, 3 }, col["a"]);
            Assert.True(actual1);
            Assert.Equal(3, col.Count);
        }

        [Fact]
        public void Test__IMultiValuedMap_PutAll_with_keyValuePairs_if_duplicateValues()
        {
            var col = NewInstance<string, int>();
            col["a"].Add(1);

            var actual1 = col.PutAll(new KeyValuePair<string, int>[]
            {
                new KeyValuePair<string, int>( "a", 1 ),
                new KeyValuePair<string, int>( "a", 1 )
            });
            Assert.Equal(new int[] { 1 }, col["a"]);
            Assert.True(actual1);
            Assert.Single(col);
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
            col["a"] = new HashSet<int> { 2, 3 };

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
            col["a"] = new HashSet<int> { 2, 3, 4 };

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
            Assert.Contains(11, actualValue);
            Assert.Contains(12, actualValue);
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

        [Theory]
        [InlineData(false, "", "a.c", "a.c")]
        [InlineData(true, "a", "a", "c")]
        public void Test_ValuesProxy_ExceptWith(bool hasKeyExpected, string srcExpected, string srcValues, string srcOtherValues)
        {
            var expected = srcExpected.Split('.').Where(s => s != "").ToArray();
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = this.NewInstance<string, string>();
            foreach (var v in values)
            {
                col.Add("key", v);
            }

            col["key"].ExceptWith(otherValues);

            var actual = col["key"];
            Assert.Equal(expected, actual);
            Assert.Equal(hasKeyExpected, col.ContainsKey("key"));
        }

        [Theory]
        [InlineData(true, "a.c", "a.c", "a.c")]
        [InlineData(false, "", "a", "c")]
        public void Test_ValuesProxy_IntersectWith(bool hasKeyExpected, string srcExpected, string srcValues, string srcOtherValues)
        {
            var expected = srcExpected.Split('.').Where(s => s != "").ToArray();
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = this.NewInstance<string, string>();
            foreach (var v in values)
            {
                col.Add("key", v);
            }

            col["key"].IntersectWith(otherValues);

            var actual = col["key"];
            Assert.Equal(expected, actual);
            Assert.Equal(hasKeyExpected, col.ContainsKey("key"));
        }

        [Theory]
        [InlineData(false, "a.c.e", "a.c.e")]
        [InlineData(true, "a.c", "a.c.e")]
        public void Test_ValuesProxy_IsProperSubsetOf(bool expected, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string, string>();
            foreach (var v in values)
            {
                col.Add("key", v);
            }

            var actual = col["key"].IsProperSubsetOf(new HashSet<string>(otherValues));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(false, "a.c.e", "a.c.e")]
        [InlineData(true, "a.c.e", "a.e")]
        public void Test_ValuesProxy_IsProperSupersetOf(bool expected, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string, string>();
            foreach (var v in values)
            {
                col.Add("key", v);
            }

            var actual = col["key"].IsProperSupersetOf(new HashSet<string>(otherValues));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true, "a.c", "a.c")]
        [InlineData(false, "a", "c")]
        public void Test_ValuesProxy_IsSubsetOf(bool expected, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string, string>();
            foreach (var v in values)
            {
                col.Add("key", v);
            }

            var actual = col["key"].IsSubsetOf(new HashSet<string>(otherValues));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true, "a.c.e", "a.c.e")]
        [InlineData(false, "a.c", "a.x")]
        public void Test_ValuesProxy_IsSupersetOf(bool expected, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string, string>();
            foreach (var v in values)
            {
                col.Add("key", v);
            }

            var actual = col["key"].IsSupersetOf(new HashSet<string>(otherValues));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true, "a.c", "b.c")]
        [InlineData(false, "a.c", "b.d")]
        public void Test_ValuesProxy_Overlaps(bool expected, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string, string>();
            foreach (var v in values)
            {
                col.Add("key", v);
            }

            var actual = col["key"].Overlaps(new HashSet<string>(otherValues));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true, "a.c", "a.c")]
        [InlineData(false, "a.c", "a.d")]
        public void Test_ValuesProxy_SetEquals(bool expected, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string, string>();
            foreach (var v in values)
            {
                col.Add("key", v);
            }

            var actual = col["key"].SetEquals(new HashSet<string>(otherValues));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(false, "", "a.c", "a.c")]
        [InlineData(true, "a", "c", "a.c")]
        public void Test_ValuesProxy_SymmetricExceptWith(bool removedExpected, string srcExcepted, string srcValues, string srcOtherValues)
        {
            var expected = srcExcepted.Split('.').Where(v => v != "").ToArray();
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string, string>();
            foreach (var v in values)
            {
                col.Add("key", v);
            }

            col["key"].SymmetricExceptWith(new HashSet<string>(otherValues));
            Assert.Equal(expected, col["key"]);
            Assert.Equal(removedExpected, col.ContainsKey("key"));
        }

        [Theory]
        [InlineData("a.c", "a", "c")]
        [InlineData("a.c", "a", "a.c")]
        public void Test_ValuesProxy_UnionWith(string srcExcepted, string srcValues, string srcOtherValues)
        {
            var expected = srcExcepted.Split('.').Where(v => v != "").ToArray();
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string, string>();
            foreach (var v in values)
            {
                col.Add("key", v);
            }

            col["key"].UnionWith(new HashSet<string>(otherValues));
            Assert.Equal(expected, col["key"]);
        }
    }
}
