using RockHouse.Collections.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RockHouse.Collections.Tests.Dictionaries
{
    public abstract class AbstractOrderedDictionaryTestBase : AbstractDictionaryTestBase
    {
        public IOrderedDictionary<K, V> NewOrderedInstance<K, V>()
        {
            return (IOrderedDictionary<K, V>)this.NewInstance<K, V>();
        }

        [Fact]
        public void Test_ICollection_CopyTo_order()
        {
            var col = NewInstance();
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
        }

        [Fact]
        public void Test_IEnumerable_GetEnumerator_order()
        {
            var col = NewInstance();
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
        }

        [Fact]
        public void Test__IHashMap_AddAll_order()
        {
            var src = NewOrderedInstance<string, int>();
            src.Add("b", 3);

            var col = NewOrderedInstance<string, int>();
            col.Add("c", 1);
            col.Add("a", 2);

            col.AddAll(src);

            Assert.Equal("c", col.FirstKey);
            Assert.Equal("b", col.LastKey);
        }

        [Fact]
        public void Test__IHashMap_Put_order()
        {
            var col = NewOrderedInstance<string, int>();

            col.Put("b", 1);
            col.Put("a", 2);
            col.Put("b", 3);

            Assert.Equal("b", col.FirstKey);
            Assert.Equal("a", col.LastKey);
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

            var col = NewOrderedInstance<string, int>();
            col.Add("b", 99);
            col.Add("c", 2);
            col.PutAll(src);

            Assert.Equal(3, col.Count);
            Assert.Equal("b", col.FirstKey);
            Assert.Equal("a", col.LastKey);
        }

        [Fact]
        public void Test__IOrdered_prop_FirstKey_if_empty()
        {
            var col = NewOrderedInstance<string, int>();

            Assert.Throws<InvalidOperationException>(() => col.FirstKey);
        }

        [Fact]
        public void Test__IOrdered_prop_LastKey()
        {
            var col = NewOrderedInstance<string, int>();
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
            var col = NewOrderedInstance<string, int>();

            Assert.Throws<InvalidOperationException>(() => col.LastKey);
        }

        [Fact]
        public void Test_Add_order()
        {
            var col = NewOrderedInstance<string, int>();
            col.Add("b", 11);
            col.Add("c", 12);
            col.Add("a", 13);

            Assert.Equal(3, col.Count);
            Assert.Equal("b", col.FirstKey);
            Assert.Equal("a", col.LastKey);
        }

        [Fact]
        public void Test_Remove_first()
        {
            var col = NewInstance();
            col.Add("d", 99);
            col.Add("c", 99);
            col.Add("b", 12);
            col.Add("a", 13);
            col.Remove("d");
            col.Remove("c");

            Assert.Equal(2, col.Count);
            Assert.Equal(new string[] { "b", "a" }, col.Keys.ToArray());
            Assert.Equal(new int[] { 12, 13 }, col.Values.ToArray());
            Assert.Equal(12, col["b"]);
            Assert.Equal(13, col["a"]);
        }

        [Fact]
        public void Test_Remove_middle()
        {
            var col = NewInstance();
            col.Add("d", 10);
            col.Add("c", 99);
            col.Add("b", 99);
            col.Add("a", 13);
            col.Remove("c");
            col.Remove("b");

            Assert.Equal(2, col.Count);
            Assert.Equal(new string[] { "d", "a" }, col.Keys.ToArray());
            Assert.Equal(new int[] { 10, 13 }, col.Values.ToArray());
            Assert.Equal(10, col["d"]);
            Assert.Equal(13, col["a"]);
        }

    }
}