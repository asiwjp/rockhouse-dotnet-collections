using RockHouse.Collections.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RockHouse.Collections.Tests.Dictionaries
{
    public abstract class AbstractLruDictionaryTestBase : AbstractDictionaryTestBase
    {
        public abstract LruDictionary<K, V> NewLruInstance<K, V>();

        public abstract LruDictionary<K, V> NewLruInstance<K, V>(int capacity);

        [Fact]
        public void Test___indexer_order()
        {
            var col = NewLruInstance<string, int>();
            col.Add("a", 1);
            col.Add("b", 2);
            col.Add("c", 2);

            // before
            Assert.Equal("a", col.FirstKey);
            Assert.Equal("c", col.LastKey);

            var tmp = col["a"];

            // after get
            Assert.Equal("b", col.FirstKey);
            Assert.Equal("a", col.LastKey);

            col["c"] = 2;

            // after set
            Assert.Equal("b", col.FirstKey);
            Assert.Equal("c", col.LastKey);
        }

        [Fact]
        public void Test_ICollection_CopyTo_order()
        {
            var col = NewLruInstance<string, int>();
            col.Add("a", 1);
            col.Add("b", 2);

            var actual = new KeyValuePair<string, int>[3];
            col.CopyTo(actual, 1);

            Assert.Null(actual[0].Key);
            Assert.Equal(0, actual[0].Value);
            Assert.Equal("a", actual[1].Key);
            Assert.Equal(1, actual[1].Value);
            Assert.Equal("b", actual[2].Key);
            Assert.Equal(2, actual[2].Value);

            // Keep order
            Assert.Equal("a", col.FirstKey);
            Assert.Equal("b", col.LastKey);
        }

        [Fact]
        public void Test__IDisposable_Dispose()
        {
            var removed = new List<ValueTuple<string, int>>();
            using (var col = NewLruInstance<string, int>())
            {
                col.Removed += (s, e) => removed.Add((e.Key, e.Value));
                col.Add("a", 1);
                col.Add("b", 2);
                col.Add("c", 3);
            }

            Assert.Equal(3, removed.Count);

            var i = 0;
            Assert.Equal(("a", 1), removed[i++]);
            Assert.Equal(("b", 2), removed[i++]);
            Assert.Equal(("c", 3), removed[i++]);
        }

        [Fact]
        public void Test__IDisposable_Dispose_if_already_disposed()
        {
            var removed = new List<ValueTuple<string, int>>();
            var col = NewLruInstance<string, int>();
            col.Removed += (s, e) => removed.Add((e.Key, e.Value));
            col.Add("a", 1);
            col.Add("b", 2);
            col.Add("c", 3);
            col.Dispose();
            Assert.Equal(3, removed.Count);

            removed.Clear();
            col.Dispose();
            Assert.Empty(removed);
        }

        [Fact]
        public void Test_IEnumerable_GetEnumerator_order()
        {
            var col = NewLruInstance<string, int>();
            col["b"] = 11;
            col["a"] = 12;

            var i = 0;
            foreach (var item in col)
            {
                switch (i++)
                {
                    case 0:
                        Assert.Equal(new KeyValuePair<string, int>("b", 11), item);
                        break;
                    case 1:
                        Assert.Equal(new KeyValuePair<string, int>("a", 12), item);
                        break;
                    default:
                        Assert.Fail("Too many items.");
                        break;
                }
            }

            // Keep order
            Assert.Equal("b", col.FirstKey);
            Assert.Equal("a", col.LastKey);
        }

        [Fact]
        public void Test__IHashMap_AddAll_order()
        {
            var src = NewLruInstance<string, int>();
            src.Add("b", 3);

            var col = NewLruInstance<string, int>();
            col.Add("c", 1);
            col.Add("a", 2);

            col.AddAll(src);

            Assert.Equal("c", col.FirstKey);
            Assert.Equal("b", col.LastKey);
        }

        [Fact]
        public void Test__IHashMap_Put_order()
        {
            var col = NewLruInstance<string, int>();

            col.Put("b", 1);
            col.Put("a", 2);
            col.Put("b", 3);

            Assert.Equal("a", col.FirstKey);
            Assert.Equal("b", col.LastKey);
        }

        [Fact]
        public void Test__IHashMap_PutAll_order()
        {
            var src = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("a", 99 ),
                new KeyValuePair<string, int>("b", 1 ),
                new KeyValuePair<string, int>("a", 3 ),
            };

            var col = NewLruInstance<string, int>();
            col.Add("b", 99);
            col.Add("c", 2);
            col.PutAll(src);

            Assert.Equal(3, col.Count);
            Assert.Equal("c", col.FirstKey);
            Assert.Equal("a", col.LastKey);
        }

        [Fact]
        public void Test__IOrdered_prop_FirstKey_if_empty()
        {
            var col = NewLruInstance<string, int>();

            Assert.Throws<InvalidOperationException>(() => col.FirstKey);
        }

        [Fact]
        public void Test__IOrdered_prop_LastKey()
        {
            var col = NewLruInstance<string, int>();
            col.Add("d", 11);
            col.Add("c", 12);
            col.Add("b", 99);
            col.Add("a", 99);

            col.Remove("a");
            col.Remove("b");

            Assert.Equal("c", col.LastKey);
        }

        [Fact]
        public void Test__IOrdered_prop_LastKey_if_empty()
        {
            var col = NewLruInstance<string, int>();

            Assert.Throws<InvalidOperationException>(() => col.LastKey);
        }

        [Fact]
        public void Test_Add_order()
        {
            var col = NewLruInstance<string, int>();
            col.Add("b", 11);
            col.Add("c", 12);
            col.Add("a", 13);

            Assert.Equal(3, col.Count);
            Assert.Equal("b", col.FirstKey);
            Assert.Equal("a", col.LastKey);
        }

        [Fact]
        public void Test_Add_if_exceeded_capacity()
        {
            var col = NewLruInstance<string, int>(2);
            col.Add("b", 11);
            col.Add("c", 12);
            col.Add("a", 13);

            Assert.Equal(2, col.Count);
            Assert.Equal("c", col.FirstKey);
            Assert.Equal("a", col.LastKey);
        }

        [Fact]
        public void Test_Remove_first()
        {
            var col = NewLruInstance<string, int>();
            col.Add("d", 99);
            col.Add("c", 99);
            col.Add("b", 12);
            col.Add("a", 13);
            col.Remove("d");
            col.Remove("c");

            // keep order
            Assert.Equal("b", col.FirstKey);
            Assert.Equal("a", col.LastKey);

            Assert.Equal(2, col.Count);
            Assert.Equal(new string[] { "b", "a" }, col.Keys.ToArray());
            Assert.Equal(new int[] { 12, 13 }, col.Values.ToArray());
            Assert.Equal(12, col["b"]);
            Assert.Equal(13, col["a"]);
        }

        [Fact]
        public void Test_Remove_middle()
        {
            var col = NewLruInstance<string, int>();
            col.Add("d", 10);
            col.Add("c", 99);
            col.Add("b", 99);
            col.Add("a", 13);
            col.Remove("c");
            col.Remove("b");

            // keep order
            Assert.Equal("d", col.FirstKey);
            Assert.Equal("a", col.LastKey);

            Assert.Equal(2, col.Count);
            Assert.Equal(new string[] { "d", "a" }, col.Keys.ToArray());
            Assert.Equal(new int[] { 10, 13 }, col.Values.ToArray());
            Assert.Equal(10, col["d"]);
            Assert.Equal(13, col["a"]);
        }


        [Fact]
        public void Test_TryGetValue_order()
        {
            var col = NewLruInstance<string, int>();
            col.Add("a", 10);
            col.Add("b", 13);

            // before
            Assert.Equal("a", col.FirstKey);
            Assert.Equal("b", col.LastKey);

            col.TryGetValue("a", out int val);

            // after
            Assert.Equal("b", col.FirstKey);
            Assert.Equal("a", col.LastKey);
        }

    }
}