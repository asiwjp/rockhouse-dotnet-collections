using RockHouse.Collections.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Dictionaries
{
    public abstract class AbstractDictionaryTestBase
    {
        public IHashMap<string, int> NewInstance() => this.NewInstance<string, int>();
        public abstract IHashMap<K, V> NewInstance<K, V>();
        public abstract IHashMap<string, int> NewInstance(int capacity);
        public abstract IHashMap<string, int> NewInstance(IEnumerable<KeyValuePair<string, int>> dictionary);
        public abstract IHashMap<K, V> NewInstance<K, V>(IEqualityComparer<K>? comparer);
        public abstract IHashMap<K, V> NewInstance<K, V>(int capacity, IEqualityComparer<K>? comparer);
        public abstract IHashMap<K, V> NewInstance<K, V>(IEnumerable<KeyValuePair<K, V>> src, IEqualityComparer<K>? comparer);
        public abstract IHashMap<K, V> Deserialize_BySystemTextJson<K, V>(string json);
        public abstract string Serialize_BySystemTextJson<K, V>(IHashMap<K, V> dictionary);

        [Fact]
        public void Test___ctor()
        {
            var col = NewInstance();
            Assert.Empty(col);
        }

        [Fact]
        public void Test___ctor_with_capacity()
        {
            NewInstance(10);
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
            Assert.Throws<ArgumentException>(() => NewInstance(new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("b", 11 ),
                new KeyValuePair<string, int>("b", 11 ),
            }));
        }

        [Fact]
        public void Test___ctor_with_equalityComparer()
        {
            var col1 = NewInstance<string, int>(new IgnoreCaseStringComparer());
            col1.Add("a", 1);
            Assert.Equal(1, col1["A"]);
            col1.Remove("A");
            Assert.Empty(col1);

            var col2 = NewInstance<string, int>(5, new IgnoreCaseStringComparer());
            col2.Add("a", 2);
            Assert.Equal(2, col2["A"]);
            col2.Remove("A");
            Assert.Empty(col2);

            var col3 = NewInstance<string, int>(new Dictionary<string, int>
            {
                { "a", 3 }
            }, new IgnoreCaseStringComparer());
            Assert.Equal(3, col3["A"]);
            col3.Remove("A");
            Assert.Empty(col3);
        }

        [Fact]
        public void Test___indexer()
        {
            var col = NewInstance();
            col.Add("a", 99);
            col["a"] = 1;

            Assert.Equal(1, col["a"]);
        }

        [Fact]
        public void Test___indexer_if_notfound()
        {
            var col = NewInstance();
            col["a"] = 1;

            Assert.Equal(1, col["a"]);
            Assert.Throws<KeyNotFoundException>(() => col["notfound"]);
        }

        [Fact]
        public void Test___withStruct()
        {
            var col = NewInstance<TestStruct, TestStruct>();
            col.Add(new TestStruct(1), new TestStruct(11));
            col[new TestStruct(1)] = new TestStruct(12);
            col[new TestStruct(2)] = new TestStruct(13);

            Assert.Equal(new TestStruct(12), col[new TestStruct(1)]);
            Assert.Equal(new TestStruct(13), col[new TestStruct(2)]);

            Assert.True(col.Remove(new TestStruct(1)));
            Assert.Single(col);
        }

        [Fact]
        public void Test__ICollection__prop_IsReadOnly()
        {
            var col = NewInstance();
            Assert.False(col.IsReadOnly);
        }

        [Fact]
        public void Test__ICollection_Clear()
        {
            var col = NewInstance();
            col.Add("a", 11);
            col.Add("b", 12);

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
            var col = NewInstance();
            col.Add("a", 1);

            var actual = col.Contains(new KeyValuePair<string, int>(key, value));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test__ICollection_CopyTo()
        {
            var col = NewInstance();
            col.Add("a", 1);
            col.Add("b", 2);

            var actual = new KeyValuePair<string, int>[3];
            col.CopyTo(actual, 1);

            Assert.Null(actual[0].Key);
            Assert.Equal(0, actual[0].Value);
            Assert.Contains(new KeyValuePair<string, int>("a", 1), actual);
            Assert.Contains(new KeyValuePair<string, int>("b", 2), actual);

            // Dictionary compatibility
            ICollection<KeyValuePair<string, int>> std = new Dictionary<string, int>(col);
            var expected = new KeyValuePair<string, int>[3];
            std.CopyTo(expected, 1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test__ICollection_CopyTo_if_index_out_of_range_at_array()
        {
            var col = NewInstance();

            var actual = new KeyValuePair<string, int>[1];
            Assert.Throws<ArgumentOutOfRangeException>(() => col.CopyTo(actual, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => col.CopyTo(actual, 2));

            // Dictionary compatibility
            ICollection<KeyValuePair<string, int>> std = new Dictionary<string, int>(col);
            var expected = new KeyValuePair<string, int>[1];
            Assert.Throws<ArgumentOutOfRangeException>(() => std.CopyTo(expected, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => std.CopyTo(expected, 2));
        }

        [Fact]
        public void Test__ICollection_CopyTo_if_index_out_of_range_at_this()
        {
            var col = NewInstance();

            var actual = new KeyValuePair<string, int>[1];
            col.CopyTo(actual, 0);
            Assert.Null(actual[0].Key);

            // Dictionary compatibility
            ICollection<KeyValuePair<string, int>> std = new Dictionary<string, int>(col);
            var expected = new KeyValuePair<string, int>[1];
            std.CopyTo(expected, 0);
            // If does not throw, ok.
        }

        [Fact]
        public void Test__ICollection_CopyTo_if_null()
        {
            var col = NewInstance();

            Assert.Throws<ArgumentNullException>(() => col.CopyTo(null, 2));

            // Dictionary compatibility
            ICollection<KeyValuePair<string, int>> std = new Dictionary<string, int>(col);
            Assert.Throws<ArgumentNullException>(() => col.CopyTo(null, 2));
        }

        [Fact]
        public void Test__ICollection_CopyTo_if_too_small()
        {
            var col = NewInstance();
            col.Add("a", 1);
            col.Add("b", 2);

            var actual = new KeyValuePair<string, int>[2];
            Assert.Throws<ArgumentException>(() => col.CopyTo(actual, 1));

            // Dictionary compatibility
            ICollection<KeyValuePair<string, int>> std = new Dictionary<string, int>(col);
            var expected = new KeyValuePair<string, int>[2];
            Assert.Throws<ArgumentException>(() => expected.CopyTo(actual, 1));
        }


        [Fact]
        public void Test__ICollection_Remove_with_keyValue()
        {
            var col = NewInstance();
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

            var col = NewInstance();
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

            var col = NewInstance();
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
            var col = NewInstance();
            col["b"] = 11;
            col["a"] = 12;


            var keyValues = col.ToArray();
            keyValues.Contains(new KeyValuePair<string, int>("b", 11));
            keyValues.Contains(new KeyValuePair<string, int>("a", 12));
        }

        [Fact]
        public void Test__IEnumerable_GetEnumerator_if_empty()
        {
            var col = NewInstance();
            Assert.Empty(col);
        }

        [Fact]
        public void Test__IHashMap_AddAll()
        {
            var src = NewInstance();
            src.Add("b", 1);
            src.Add("a", 2);

            var col = NewInstance();
            col.AddAll(src);

            Assert.Equal(2, col.Count);
            Assert.Equal(2, col["a"]);
            Assert.Equal(1, col["b"]);
        }

        [Fact]
        public void Test__IHashMap_AddAll_if_key_already_exists()
        {
            var src = NewInstance();
            src.Add("b", 1);

            var col = NewInstance();
            col.Add("b", 99);
            col.Add("a", 1);

            Assert.Throws<ArgumentException>(() => col.AddAll(src));
        }

        [Fact]
        public void Test__IHashMap_AddAll_if_keys_are_duplicated_in_the_source()
        {
            var src = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("a", 1 ),
                new KeyValuePair<string, int>("b", 1 ),
                new KeyValuePair<string, int>("a", 2 ),
            };

            var col = NewInstance();
            Assert.Throws<ArgumentException>(() => col.AddAll(src));
        }

        [Fact]
        public void Test__IHashMap_Get()
        {
            var col = NewInstance();
            col.Add("a", 1);

            Assert.Equal(1, col.Get("a"));
        }

        [Fact]
        public void Test__IHashMap_Get_primitiveType_if_notfound()
        {
            var col = NewInstance();

            Assert.Equal(0, col.Get("a"));
        }

        [Fact]
        public void Test__IHashMap_Get_nullablePrimitiveType_if_notfound()
        {
            var col = NewInstance<string, int?>();

            Assert.Null(col.Get("a"));
        }

        [Fact]
        public void Test__IHashMap_Get_refType_if_notfound()
        {
            var col = NewInstance<string, string>();

            Assert.Null(col.Get("a"));
        }

        [Fact]
        public void Test__IHashMap_Get_withCallback()
        {
            static int ifNotFound(int key)
            {
                return key + 1000;
            }

            static int ifFound(int key, int old)
            {
                return key + old + 2000;
            }

            var col = NewInstance<int, int>();
            col.Add(100, 1);

            var actual1 = col.Get(100, ifNotFound: null, ifFound: null);
            Assert.Equal(1, actual1);

            var actual2 = col.Get(999, ifNotFound: null, ifFound: null);
            Assert.Equal(0, actual2);

            var actual3 = col.Get(100, ifNotFound: ifNotFound, ifFound: ifFound);
            Assert.Equal(2101, actual3);

            var actual4 = col.Get(999, ifNotFound: ifNotFound, ifFound: ifFound);
            Assert.Equal(1999, actual4);
        }

        [Fact]
        public void Test__IHashMap_Delete()
        {
            var col = NewInstance<int, int>();
            col.Add(100, 1);

            var actual = col.Delete(100);
            Assert.Equal(1, actual);
            Assert.False(col.ContainsKey(100));
        }

        [Fact]
        public void Test__IHashMap_Delete_if_notfound()
        {
            var col = NewInstance<int, int>();
            col.Add(100, 1);

            var actual = col.Delete(999);
            Assert.Equal(0, actual);
        }

        [Fact]
        public void Test__IHashMap_Delete_nullablePrimitiveType_if_notfound()
        {
            var col = NewInstance<int, int?>();
            col.Add(100, 1);

            var actual = col.Delete(999);
            Assert.Null(actual);
        }

        [Fact]
        public void Test__IHashMap_Delete_refType_if_notfound()
        {
            var col = NewInstance<int, string>();
            col.Add(100, "a");

            var actual = col.Delete(999);
            Assert.Null(actual);
        }

        [Fact]
        public void Test__IHashMap_Delete_withCallback()
        {
            static int ifNotFound(int key)
            {
                return key + 1000;
            }

            static int ifFound(int key, int old)
            {
                return key + old + 2000;
            }

            var col = NewInstance<int, int>();
            col.Add(100, 1);
            col.Add(101, 1);

            var actual1 = col.Delete(100, ifNotFound: null, ifFound: null);
            Assert.Equal(1, actual1);

            var actual2 = col.Delete(100, ifNotFound: null, ifFound: null);
            Assert.Equal(0, actual2);

            var actual3 = col.Delete(101, ifNotFound: ifNotFound, ifFound: ifFound);
            Assert.Equal(2102, actual3);

            var actual4 = col.Delete(101, ifNotFound: ifNotFound, ifFound: ifFound);
            Assert.Equal(1101, actual4);
        }

        [Fact]
        public void Test__IHashMap_Put()
        {
            var col = NewInstance();

            int actual1 = col.Put("b", 1);
            Assert.Equal(1, col["b"]);
            Assert.Equal(0, actual1);

            int actual2 = col.Put("a", 2);
            Assert.Equal(1, col["b"]);
            Assert.Equal(2, col["a"]);
            Assert.Equal(0, actual2);

            var actual3 = col.Put("b", 3);
            Assert.Equal(3, col["b"]);
            Assert.Equal(1, actual3);
        }

        [Fact]
        public void Test__IHashMap_Put_nullable()
        {
            var col = NewInstance<string, int?>();

            var actual1 = col.Put("a", 1);
            Assert.Equal(1, col["a"]);
            Assert.Null(actual1);

            var actual2 = col.Put("a", 2);
            Assert.Equal(2, col["a"]);
            Assert.Equal(1, actual2);
        }

        [Fact]
        public void Test__IHashMap_Put_withCallback()
        {
            static int ifNotFound(int key)
            {
                return key + 1000;
            }

            static int ifFound(int key, int old)
            {
                return key + old + 2000;
            }

            var col = NewInstance<int, int>();

            var actual1 = col.Put(100, 1, ifNotFound: null, ifFound: null);
            Assert.Equal(1, col[100]);
            Assert.Equal(0, actual1);

            var actual2 = col.Put(100, 2, ifNotFound: null, ifFound: null);
            Assert.Equal(2, col[100]);
            Assert.Equal(1, actual2);

            var actual3 = col.Put(200, 1, ifNotFound: ifNotFound, ifFound: ifFound);
            Assert.Equal(1, col[200]);
            Assert.Equal(1200, actual3);

            var actual4 = col.Put(200, 2, ifNotFound: ifNotFound, ifFound: ifFound);
            Assert.Equal(2, col[200]);
            Assert.Equal(2201, actual4);
        }

        [Fact]
        public void Test__IHashMap_PutAll()
        {
            var src = NewInstance();
            src.Add("b", 1);
            src.Add("a", 2);

            var col = NewInstance();
            col.PutAll(src);

            Assert.Equal(2, col.Count);
            Assert.Equal(2, col["a"]);
            Assert.Equal(1, col["b"]);
        }

        [Fact]
        public void Test__IHashMap_PutAll_if_key_already_exists()
        {
            var src = NewInstance();
            src.Add("b", 1);

            var col = NewInstance();
            col.Add("b", 99);
            col.Add("a", 2);
            col.PutAll(src);

            Assert.Equal(2, col.Count);
            Assert.Equal(2, col["a"]);
            Assert.Equal(1, col["b"]);
        }

        [Fact]
        public void Test__IHashMap_PutAll_if_keys_are_duplicated_in_the_source()
        {
            var src = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("a", 1 ),
                new KeyValuePair<string, int>("b", 1 ),
                new KeyValuePair<string, int>("a", 2 ),
            };

            var col = NewInstance();
            col.PutAll(src);

            Assert.Equal(2, col.Count);
            Assert.Equal(2, col["a"]);
        }

        [Fact]
        public void Test__IHashMap_PutIfAbsent()
        {
            var col = NewInstance();

            int actual1 = col.PutIfAbsent("a", 1);
            Assert.Equal(1, col["a"]);
            Assert.Equal(0, actual1);

            int actual2 = col.PutIfAbsent("a", 2);
            Assert.Equal(1, col["a"]);
            Assert.Equal(1, actual2);
        }

        [Fact]
        public void Test__Object_ToString()
        {
            var col = NewInstance();
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
            };

            var actualSystem = this.Deserialize_BySystemTextJson<string, int>(@"{""b"":1,""a"":2}");
            Assert.Equal(expected, actualSystem.ToArray());
        }

        [Fact]
        public void Test__json_Serialize()
        {
            var expected = @"{""b"":1,""a"":2}";

            var col = NewInstance<string, int>();
            col.Add("b", 1);
            col.Add("a", 2);

            var actualSystem = this.Serialize_BySystemTextJson(col);
            Assert.Equal(expected, actualSystem);
        }

        [Fact]
        public void Test__prop_Keys()
        {
            var col = NewInstance();
            col.Add("a", 1);
            col.Add("b", 1);
            col["a"] = 1;

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
            var col = NewInstance();
            col.Add("a", 3);
            col.Add("b", 1);
            col["a"] = 2;

            var values = col.Values.ToList();
            Assert.Equal(2, values.Count);
            Assert.Contains(1, values);
            Assert.Contains(2, values);

            // is immutable
            Assert.Throws<NotSupportedException>(() => col.Values.Remove(1));
        }

        [Fact]
        public void Test_Add_canAdd()
        {
            var col = NewInstance();
            col.Add("b", 11);
            col.Add("a", 12);

            Assert.Equal(2, col.Count);
            Assert.Contains(new KeyValuePair<string, int>("a", 12), col);
            Assert.Contains(new KeyValuePair<string, int>("b", 11), col);
        }

        [Fact]
        public void Test_Add_if_already_exists()
        {
            var col = NewInstance();
            col.Add("a", 11);

            Assert.Throws<ArgumentException>(() => col.Add("a", 99));
        }

        [Fact]
        public void Test_Add_keyValuePair()
        {
            var col = NewInstance();
            col.Add(new KeyValuePair<string, int>("a", 11));
            Assert.Equal(11, col["a"]);
        }

        [Fact]
        public void Test_ContainsKey_if_exists()
        {
            var col = this.NewInstance();
            col.Add("a", 1);

            var actual = col.ContainsKey("a");

            Assert.True(actual);
        }

        [Fact]
        public void Test_ContainsKey_if_notfound()
        {
            var col = NewInstance();

            var actual = col.ContainsKey("notfound");

            Assert.False(actual);
        }

        [Fact]
        public void Test_Remove_canRemove()
        {
            var col = NewInstance();
            col.Add("a", 11);
            col.Add("b", 12);

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
            var col = NewInstance();

            col.Remove("notfound");
            Assert.Empty(col);
        }

        [Fact]
        public void Test_Remove_if_notfound()
        {
            var col = this.NewInstance();
            col.Add("a", 11);

            col.Remove("notfound");
            Assert.Single(col);
            Assert.Contains<string>("a", col.Keys);
        }

        [Fact]
        public void Test_TryGetValue_canGet()
        {
            var col = this.NewInstance();
            col.Add("a", 11);

            var actualRet = col.TryGetValue("a", out int actualValue);
            Assert.True(actualRet);
            Assert.Equal(11, actualValue);
        }

        [Fact]
        public void Test_TryGetValue_if_empty()
        {
            var col = NewInstance();

            var actualRet = col.TryGetValue("notfound", out int actualValue);
            Assert.False(actualRet);
            Assert.Equal(default, actualValue);
        }

        [Fact]
        public void Test_TryGetValue_if_notfound()
        {
            var col = this.NewInstance();
            col.Add("a", 11);

            var actualRet = col.TryGetValue("notfound", out int actualValue);
            Assert.False(actualRet);
            Assert.Equal(default, actualValue);
        }
    }
}