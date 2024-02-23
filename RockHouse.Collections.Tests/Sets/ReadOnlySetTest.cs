using RockHouse.Collections.Sets;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Sets
{
    public class ReadOnlySetTest
    {
        public ReadOnlySet<T> NewInstance<T>() => new ReadOnlySet<T>();
        public ReadOnlySet<T> NewInstance<T>(ISet<T> src) => new ReadOnlySet<T>(src);

        [Fact]
        public void Test__withStruct()
        {
            var col = NewInstance<TestStruct>();
            col.Add(new TestStruct(1));
            Assert.False(col.Add(new TestStruct(1)));
            Assert.True(col.Add(new TestStruct(2)));

            Assert.Contains(new TestStruct(1), col);

            Assert.True(col.Remove(new TestStruct(1)));
            Assert.Single(col);
        }

        [Fact]
        public void Test___ctor()
        {
            var col = NewInstance<string>();
            Assert.Empty(col);
        }

        [Fact]
        public void Test___ctor_with_iset()
        {
            var col = NewInstance<string>(new HashSet<string> { "b", "a" });

            Assert.Equal(2, col.Count);
            Assert.Equal(new string[] { "b", "a" }, col.ToArray());
        }

        [Fact]
        public void Test__ICollection__prop_Count()
        {
            var col = NewInstance<string>(new HashSet<string>(new string[] { "a" }));
            Assert.Equal(1, col.Count);
        }

        [Fact]
        public void Test__ICollection__prop_IsReadOnly()
        {
            var col = NewInstance<string>();
            Assert.True(col.IsReadOnly);
        }

        [Fact]
        public void Test__ICollection_Add()
        {
            ICollection<string> col = NewInstance<string>();
            Assert.Throws<NotSupportedException>(() => col.Add("a"));
        }

        [Fact]
        public void Test__ICollection_Clear()
        {
            ICollection<string> col = NewInstance<string>();
            Assert.Throws<NotSupportedException>(() => col.Clear());
        }

        [Fact]
        public void Test__ICollection_Contains_if_exists()
        {
            var col = this.NewInstance<string>(new HashSet<string>(new string[] { "a" }));

            var actual = col.Contains("a");

            Assert.True(actual);
        }

        [Fact]
        public void Test__ICollection_Contains_if_notfound()
        {
            var col = this.NewInstance<string>(new HashSet<string>(new string[] { "a" }));

            var actual = col.Contains("missing");

            Assert.False(actual);
        }

        [Fact]
        public void Test__ICollection_Remove()
        {
            var col = this.NewInstance<string>();
            Assert.Throws<NotSupportedException>(() => col.Remove(""));
        }

        [Fact]
        public void Test__IEnumerable_GetEnumerator()
        {
            var col = NewInstance<string>(new HashSet<string>(new string[] { "b", "a" }));

            var i = 0;
            foreach (var item in col)
            {
                switch (i++)
                {
                    case 0:
                        Assert.Equal("b", item);
                        break;
                    case 1:
                        Assert.Equal("a", item);
                        break;
                    default:
                        Assert.Fail("Too many items.");
                        break;
                }
            }
        }
    }
}