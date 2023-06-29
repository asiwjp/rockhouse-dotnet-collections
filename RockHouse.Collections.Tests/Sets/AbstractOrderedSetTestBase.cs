using RockHouse.Collections.Sets;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Sets
{
    public abstract class AbstractOrderedSetTestBase
    {
        public AbstractOrderedSet<string> NewInstance() => this.NewInstance<string>();
        public abstract AbstractOrderedSet<T> NewInstance<T>();
        public abstract AbstractOrderedSet<T> NewInstance<T>(int capacity);
        public abstract AbstractOrderedSet<string> NewInstance(IEnumerable<string> src);
        public abstract AbstractOrderedSet<string> NewInstance(ISet<string> src);
        public abstract AbstractOrderedSet<T> NewInstance<T>(IEqualityComparer<T>? comparer);
        public abstract AbstractOrderedSet<T> NewInstance<T>(int capacity, IEqualityComparer<T>? comparer);
        public abstract AbstractOrderedSet<T> NewInstance<T>(IEnumerable<T> src, IEqualityComparer<T>? comparer);
        public abstract AbstractOrderedSet<T> Deserialize_BySystemTextJson<T>(string json);
        public abstract string Serialize_BySystemTextJson<T>(AbstractOrderedSet<T> src);

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
            var col = NewInstance();
            Assert.Empty(col);
        }

        [Fact]
        public void Test___ctor_with_capacity()
        {
            var col = NewInstance<string>(10);
            // If does not thorw exception, ok.
        }

        [Fact]
        public void Test___ctor_with_enumerable()
        {
            var col = NewInstance(new string[] { "b", "a" });

            Assert.Equal(2, col.Count);
            Assert.Equal(new string[] { "b", "a" }, col.ToArray());
        }

        [Fact]
        public void Test___ctor_with_collection()
        {
            var col = NewInstance(new HashSet<string> { "b", "a" });

            Assert.Equal(2, col.Count);
            Assert.Equal(new string[] { "b", "a" }, col.ToArray());
        }

        [Fact]
        public void Test___equalityComparer()
        {
            var col1 = NewInstance<string>(new IgnoreCaseStringComparer());
            col1.Add("a");
            Assert.Contains("a", col1);
            Assert.Contains("A", col1);

            Assert.False(col1.Add("A"));
            Assert.Equal("a", col1.First);

            Assert.True(col1.Remove("A"));
            Assert.Empty(col1);

            var col2 = NewInstance<string>(5, new IgnoreCaseStringComparer());
            col2.Add("a");
            Assert.Contains("a", col2);
            Assert.Contains("A", col2);

            var col3 = NewInstance<string>(new string[] { "A" }, new IgnoreCaseStringComparer());
            col2.Add("a");
            Assert.Contains("a", col2);
            Assert.Contains("A", col2);
        }

        [Fact]
        public void Test__ICollection__prop_IsReadOnly()
        {
            var col = NewInstance();
            Assert.False(col.IsReadOnly);
        }

        [Fact]
        public void Test__ICollection_Add()
        {
            ICollection<string> col = NewInstance();
            col.Add("a");

            Assert.Equal("a", col.First());
        }

        [Fact]
        public void Test__ICollection_Add_canAdd()
        {
            var col = NewInstance();
            var atual1 = col.Add("b");
            var atual2 = col.Add("a");

            Assert.True(atual1);
            Assert.True(atual2);
            Assert.Equal(2, col.Count);
            Assert.Equal(new string[] { "b", "a" }, col.ToArray());
        }

        [Fact]
        public void Test__ICollection_Add_if_already_exists()
        {
            var col = NewInstance();
            col.Add("a");
            var actual = col.Add("a");

            Assert.False(actual);
            Assert.Single(col);
            Assert.Equal(new string[] { "a" }, col.ToArray());
        }


        [Fact]
        public void Test__ICollection_Clear()
        {
            var col = NewInstance();
            col.Add("a");
            col.Add("b");

            col.Clear();
            Assert.Empty(col);

            col.Add("c");
            Assert.Equal("c", col.First);
        }

        [Fact]
        public void Test__ICollection_Contains_if_exists()
        {
            var col = this.NewInstance();
            col.Add("a");

            var actual = col.Contains("a");

            Assert.True(actual);
        }

        [Fact]
        public void Test__ICollection_Contains_if_notfound()
        {
            var col = NewInstance();

            var actual = col.Contains("notfound");

            Assert.False(actual);
        }


        [Fact]
        public void Test__ICollection_CopyTo()
        {
            var col = NewInstance();
            col.Add("a");
            col.Add("b");

            var actual = new string[3];
            col.CopyTo(actual, 1);

            Assert.Null(actual[0]);
            Assert.Equal("a", actual[1]);
            Assert.Equal("b", actual[2]);

            // HashSet compatiblity
            var std = new HashSet<string>(col);
            var expected = new string[3];
            std.CopyTo(expected, 1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test__ICollection_CopyTo_if_index_out_of_range_at_array()
        {
            var col = NewInstance();

            var actual = new string[1];
            Assert.Throws<ArgumentOutOfRangeException>(() => col.CopyTo(actual, -1));
            Assert.Throws<ArgumentException>(() => col.CopyTo(actual, 2));

            // HashSet compatiblity
            var std = new HashSet<string>(col);
            Assert.Throws<ArgumentOutOfRangeException>(() => std.CopyTo(actual, -1));
            Assert.Throws<ArgumentException>(() => std.CopyTo(actual, 2));
        }

        [Fact]
        public void Test__ICollection_CopyTo_if_index_out_of_range_at_this()
        {
            var col = NewInstance();

            var actual = new string[1];
            col.CopyTo(actual, 0);
            Assert.Null(actual[0]);

            // HashSet compatiblity
            var std = new HashSet<string>(col);
            var expected = new string[1];
            std.CopyTo(expected, 0);
            // If does not thorw, ok
        }

        [Fact]
        public void Test__ICollection_CopyTo_if_null()
        {
            var col = NewInstance();
            Assert.Throws<ArgumentNullException>(() => col.CopyTo(null, 2));

            // HashSet compatiblity
            var std = new HashSet<string>(col);
            Assert.Throws<ArgumentNullException>(() => std.CopyTo(null, 2));
        }

        [Fact]
        public void Test__ICollection_CopyTo_if_too_small()
        {
            var col = NewInstance();
            col.Add("a");
            col.Add("b");

            var actual = new string[2];
            Assert.Throws<ArgumentException>(() => col.CopyTo(actual, 1));

            // HashSet compatiblity
            var std = new HashSet<string>(col);
            Assert.Throws<ArgumentException>(() => std.CopyTo(actual, 1));
        }

        [Fact]
        public void Test__ICollection_Remove_canRemove()
        {
            var col = this.NewInstance();
            col.Add("a");
            col.Add("b");

            col.Remove("a");
            Assert.Single(col);
            Assert.DoesNotContain<string>("a", col.ToArray());

            col.Remove("b");
            Assert.Empty(col);
            Assert.DoesNotContain<string>("b", col.ToArray());
        }

        [Fact]
        public void Test__ICollection_Remove_if_empty()
        {
            var col = NewInstance();

            col.Remove("notfound");
            Assert.Empty(col);
        }

        [Fact]
        public void Test__ICollection_Remove_if_notfound()
        {
            var col = this.NewInstance();
            col.Add("a");

            col.Remove("notfound");
            Assert.Single(col);
            Assert.Contains<string>("a", col);
        }

        [Fact]
        public void Test__ICollection_Remove_first()
        {
            var col = NewInstance();
            col.Add("d");
            col.Add("c");
            col.Add("b");
            col.Add("a");
            col.Remove("d");
            col.Remove("c");

            Assert.Equal(2, col.Count);
            Assert.Equal(new string[] { "b", "a" }, col.ToArray());
        }

        [Fact]
        public void Test__ICollection_Remove_middle()
        {
            var col = NewInstance();
            col.Add("d");
            col.Add("c");
            col.Add("b");
            col.Add("a");
            col.Remove("c");
            col.Remove("b");

            Assert.Equal(2, col.Count);
            Assert.Equal(new string[] { "d", "a" }, col.ToArray());
        }

        [Fact]
        public void Test__IEnumerable_GetEnumerator()
        {
            var col = NewInstance();
            col.Add("b");
            col.Add("a");

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

        [Fact]
        public void Test__IEnumerable_GetEnumerator_if_empty()
        {
            var col = NewInstance();
            Assert.Empty(col);
        }

        [Fact]
        public void Test__IHashSet_AddAll()
        {
            var src = new List<string>
            {
                "b",
                "a"
            };

            var col = NewInstance();
            var actual = col.AddAll(src);

            Assert.True(actual);
            Assert.Equal(new string[] { "b", "a" }, col.ToArray());
        }

        [Fact]
        public void Test__IHashSet_AddAll_if_already_exists()
        {
            var src = new List<string>
            {
                "b",
                "a"
            };

            var col = NewInstance();
            col.Add("a");
            var actual = col.AddAll(src);

            Assert.True(actual);
            Assert.Equal(new string[] { "a", "b" }, col.ToArray());
        }

        [Fact]
        public void Test__IHashSet_AddAll_if_not_changed()
        {
            var src = new List<string>
            {
                "a"
            };

            var col = NewInstance();
            col.Add("a");
            var actual = col.AddAll(src);

            Assert.False(actual);
            Assert.Equal(new string[] { "a" }, col.ToArray());
        }

        [Fact]
        public void Test__IHashSet_AddAll_if_duplicate_elements_the_src()
        {
            var src = new List<string>
            {
                "b",
                "b",
                "b"
            };

            var col = NewInstance();
            var actual = col.AddAll(src);

            Assert.True(actual);
            Assert.Equal(new string[] { "b" }, col.ToArray());
        }

        [Fact]
        public void Test__IOrdered_prop_First()
        {
            var col = NewInstance();
            col.Add("c");
            col.Add("b");
            col.Add("a");
            col.Remove("c");
            col.Remove("b");

            Assert.Equal("a", col.First);
        }

        [Fact]
        public void Test__IOrdered_prop_First_if_empty()
        {
            var col = NewInstance();

            Assert.Throws<InvalidOperationException>(() => col.First);
        }

        [Fact]
        public void Test__IOrdered_prop_Last()
        {
            var col = NewInstance();
            col.Add("d");
            col.Add("c");
            col.Add("b");
            col.Add("a");

            col.Remove("a");
            col.Remove("b");

            Assert.Equal("c", col.Last);
        }

        [Fact]
        public void Test__IOrdered_prop_Last_if_empty()
        {
            var col = NewInstance();

            Assert.Throws<InvalidOperationException>(() => col.Last);
        }

        [Fact]
        public void Test__json_SystemTextJson_Serialize()
        {
            var col = NewInstance<string>();
            col.Add("b");
            col.Add("a");
            var actual = this.Serialize_BySystemTextJson(col);
            Assert.Equal(@"[""b"",""a""]", actual);
        }

        [Fact]
        public void Test__json_SystemTextJson_Deserialize()
        {
            var actual = this.Deserialize_BySystemTextJson<string>(@"[""b"",""a""]");

            Assert.Equal(new string[]
            {
                "b",
                "a"
            }, actual.ToArray());
        }

        [Fact]
        public void Test__Object_ToString()
        {
            var col = NewInstance();
            col.Add("");

            Assert.Equal("Count=1", col.ToString());
        }

        [Theory]
        [InlineData("", "a.c", "a.c")]
        [InlineData("", "c", "a.c")]
        [InlineData("a", "a.c", "c")]
        [InlineData("", "", "a.c")]
        [InlineData("a.c", "a.c", "")]
        [InlineData("", "", "")]
        public void Test_ExceptWith(string srcExpected, string srcValues, string srcOtherValues)
        {
            var expected = srcExpected.Split('.').Where(s => s != "").ToArray();
            var values = srcValues.Split('.').Where(s => s != "").ToArray();
            var otherValues = srcOtherValues.Split('.').Where(s => s != "").ToArray();

            var col = NewInstance();
            col.AddAll(values);

            col.ExceptWith(otherValues);
            var actual = col.ToArray();

            Assert.Equal(expected, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            std.ExceptWith(otherValues);
            var stdResult = std.ToArray();
            Assert.Equal(stdResult, actual);
        }

        [Fact]
        public void Test_ExceptWith_if_null()
        {
            var col = NewInstance();
            Assert.Throws<ArgumentNullException>(() => col.ExceptWith(null));
        }

        [Theory]
        [InlineData("", "a", "A")]
        [InlineData("", "A", "a")]
        public void Test_ExceptWith_with_equalityComparer(string srcExpected, string srcValues, string srcOtherValues)
        {
            var expected = srcExpected.Split('.').Where(s => s != "").ToArray();
            var values = srcValues.Split('.').Where(s => s != "").ToArray();
            var otherValues = srcOtherValues.Split('.').Where(s => s != "").ToArray();

            var col = NewInstance(new IgnoreCaseStringComparer());
            col.AddAll(values);

            col.ExceptWith(otherValues);
            var actual = col.ToArray();

            Assert.Equal(expected, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values, new IgnoreCaseStringComparer());
            std.ExceptWith(otherValues);
            var stdResult = std.ToArray();
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData("a.c", "a.c", "a.c")]
        [InlineData("c", "c", "a.c")]
        [InlineData("c", "a.c", "c")]
        [InlineData("", "", "a.c")]
        [InlineData("", "a.c", "")]
        [InlineData("", "", "")]
        public void Test_IntersectWith(string srcExpected, string srcValues, string srcOtherValues)
        {
            var expected = srcExpected.Split('.').Where(s => s != "").ToArray();
            var values = srcValues.Split('.').Where(s => s != "").ToArray();
            var otherValues = srcOtherValues.Split('.').Where(s => s != "").ToArray();

            var col = NewInstance();
            col.AddAll(values);

            col.IntersectWith(otherValues);
            var actual = col.ToArray();

            Assert.Equal(expected, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            std.IntersectWith(otherValues);
            var stdResult = std.ToArray();
            Assert.Equal(stdResult, actual);
        }

        [Fact]
        public void Test_IntersectWith_if_null()
        {
            var col = NewInstance();
            Assert.Throws<ArgumentNullException>(() => col.IntersectWith(null));
        }

        [Fact]
        public void Test_IntersectWith_using_HashSet()
        {
            var expected = new string[] { "c" };
            var values = new string[] { "c" };
            var otherValues = new HashSet<string>(new string[] { "c" });

            var col = NewInstance();
            col.AddAll(values);

            col.IntersectWith(otherValues);
            var actual = col.ToArray();

            Assert.Equal(expected, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            std.IntersectWith(otherValues);
            var stdResult = std.ToArray();
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData("a", "a", "A")]
        [InlineData("A", "A", "a")]
        [InlineData("", "a", "c")]
        public void Test_IntersectWith_with_equalityComparer(string srcExpected, string srcValues, string srcOtherValues)
        {
            var expected = srcExpected.Split('.').Where(s => s != "").ToArray();
            var values = srcValues.Split('.').Where(s => s != "").ToArray();
            var otherValues = srcOtherValues.Split('.').Where(s => s != "").ToArray();

            var col = NewInstance<string>(new IgnoreCaseStringComparer());
            col.AddAll(values);

            col.IntersectWith(otherValues);
            var actual = col.ToArray();

            Assert.Equal(expected, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values, new IgnoreCaseStringComparer());
            std.IntersectWith(otherValues);
            var stdResult = std.ToArray();
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData(false, "a.c.e", "a.c.e")]
        [InlineData(true, "a.c", "a.c.e")]
        [InlineData(false, "a.c.e", "a.c")]
        [InlineData(true, "", "a.c.e")]
        [InlineData(false, "a.c.e", "")]
        public void Test_IsProperSubsetOf(bool excepted, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance();
            col.AddAll(values);

            var actual = col.IsProperSubsetOf(otherValues);
            Assert.Equal(excepted, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            var stdResult = std.IsProperSubsetOf(otherValues);
            Assert.Equal(stdResult, actual);
        }

        [Fact]
        public void Test_IsProperSubsetOf_if_null()
        {
            var col = NewInstance();
            Assert.Throws<ArgumentNullException>(() => col.IsProperSubsetOf(null));
        }

        [Fact]
        public void Test_IsProperSubsetOf_if_equals_ref()
        {
            var values = new string[] { "a", "c", "e" };
            var col = NewInstance();
            col.AddAll(values);

            var actual = col.IsProperSubsetOf(col);
            Assert.False(actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            var stdResult = std.IsProperSubsetOf(std);
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData(true, "a", "A.c")]
        [InlineData(true, "A", "a.c")]
        public void Test_IsProperSubsetOf_with_equalityComparer(bool excepted, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string>(new IgnoreCaseStringComparer());
            col.AddAll(values);

            var actual = col.IsProperSubsetOf(otherValues);
            Assert.Equal(excepted, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values, new IgnoreCaseStringComparer());
            var stdResult = std.IsProperSubsetOf(otherValues);
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData(false, "a.c.e", "a.c.e")]
        [InlineData(false, "a.c", "a.c.e")]
        [InlineData(true, "a.c.e", "a.c")]
        [InlineData(false, "", "a.c.e")]
        [InlineData(true, "a.c.e", "")]
        public void Test_IsProperSupersetOf(bool excepted, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance();
            col.AddAll(values);

            var actual = col.IsProperSupersetOf(otherValues);
            Assert.Equal(excepted, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            var stdResult = std.IsProperSupersetOf(otherValues);
            Assert.Equal(stdResult, actual);
        }

        [Fact]
        public void Test_IsProperSupersetOf_if_null()
        {
            var col = NewInstance();
            Assert.Throws<ArgumentNullException>(() => col.IsProperSupersetOf(null));
        }

        [Fact]
        public void Test_IsProperSupersetOf_if_equals_ref()
        {
            var values = new string[] { "a", "c", "e" };
            var col = NewInstance();
            col.AddAll(values);

            var actual = col.IsProperSupersetOf(col);
            Assert.False(actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            var stdResult = std.IsProperSupersetOf(std);
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData(true, "a.c", "A")]
        [InlineData(true, "A.c", "a")]
        public void Test_IsProperSupersetOf_with_equalityComparer(bool excepted, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string>(new IgnoreCaseStringComparer());
            col.AddAll(values);

            var actual = col.IsProperSupersetOf(otherValues);
            Assert.Equal(excepted, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values, new IgnoreCaseStringComparer());
            var stdResult = std.IsProperSupersetOf(otherValues);
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData(true, "a.c.e", "a.c.e")]
        [InlineData(true, "a.c", "a.c.e")]
        [InlineData(false, "a.c.e", "a.c")]
        [InlineData(false, "f", "a.c")]
        [InlineData(false, "a", "c")]
        [InlineData(true, "", "a.c.e")]
        [InlineData(false, "a.c.e", "")]
        public void Test_IsSubsetOf(bool excepted, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance();
            col.AddAll(values);

            var actual = col.IsSubsetOf(otherValues);
            Assert.Equal(excepted, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            var stdResult = std.IsSubsetOf(otherValues);
            Assert.Equal(stdResult, actual);
        }

        [Fact]
        public void Test_IsSubsetOf_if_null()
        {
            var col = NewInstance();
            Assert.Throws<ArgumentNullException>(() => col.IsSubsetOf(null));
        }

        [Fact]
        public void Test_IsSubsetOf_if_equals_ref()
        {
            var values = new string[] { "a", "c", "e" };
            var col = NewInstance();
            col.AddAll(values);

            var actual = col.IsSubsetOf(col);
            Assert.True(actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            var stdResult = std.IsSubsetOf(std);
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData(true, "a", "A.c")]
        [InlineData(true, "A", "a.c")]
        public void Test_IsSubsetOf_with_equalityComparer(bool excepted, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string>(new IgnoreCaseStringComparer());
            col.AddAll(values);

            var actual = col.IsSubsetOf(otherValues);
            Assert.Equal(excepted, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values, new IgnoreCaseStringComparer());
            var stdResult = std.IsSubsetOf(otherValues);
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData(true, "a.c.e", "a.c.e")]
        [InlineData(false, "a.c", "a.c.e")]
        [InlineData(true, "a.c.e", "a.c")]
        [InlineData(false, "", "a.c.e")]
        [InlineData(true, "a.c.e", "")]
        public void Test_IsSupersetOf(bool excepted, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance();
            col.AddAll(values);

            var actual = col.IsSupersetOf(otherValues);
            Assert.Equal(excepted, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            var stdResult = std.IsSupersetOf(otherValues);
            Assert.Equal(stdResult, actual);
        }

        [Fact]
        public void Test_IsSupersetOf_if_null()
        {
            var col = NewInstance();
            Assert.Throws<ArgumentNullException>(() => col.IsSupersetOf(null));
        }

        [Fact]
        public void Test_IsSupersetOf_if_equals_ref()
        {
            var values = new string[] { "a", "c", "e" };
            var col = NewInstance();
            col.AddAll(values);

            var actual = col.IsSupersetOf(col);
            Assert.True(actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            var stdResult = std.IsSupersetOf(std);
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData(true, "a.c", "A")]
        [InlineData(true, "A.c", "a")]
        public void Test_IsSupersetOf_with_equalityComparer(bool excepted, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string>(new IgnoreCaseStringComparer());
            col.AddAll(values);

            var actual = col.IsSupersetOf(otherValues);
            Assert.Equal(excepted, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values, new IgnoreCaseStringComparer());
            var stdResult = std.IsSupersetOf(otherValues);
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData(true, "a.c", "a.c")]
        [InlineData(true, "c", "a.c")]
        [InlineData(true, "a.c", "c")]
        [InlineData(false, "", "a.c")]
        [InlineData(false, "a.c", "")]
        public void Test_Overlaps(bool excepted, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance();
            col.AddAll(values);

            var actual = col.Overlaps(otherValues);
            Assert.Equal(excepted, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            var stdResult = std.Overlaps(otherValues);
            Assert.Equal(stdResult, actual);
        }

        [Fact]
        public void Test_Overlaps_if_null()
        {
            var col = NewInstance();
            Assert.Throws<ArgumentNullException>(() => col.Overlaps(null));
        }

        [Fact]
        public void Test_Overlaps_if_equals_ref()
        {
            var values = new string[] { "a", "c", "e" };
            var col = NewInstance();
            col.AddAll(values);

            var actual = col.Overlaps(col);
            Assert.True(actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            var stdResult = std.Overlaps(std);
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData(true, "a", "A")]
        [InlineData(true, "A", "a")]
        public void Test_Overlaps_with_equalityComparer(bool excepted, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string>(new IgnoreCaseStringComparer());
            col.AddAll(values);

            var actual = col.Overlaps(otherValues);
            Assert.Equal(excepted, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values, new IgnoreCaseStringComparer());
            var stdResult = std.Overlaps(otherValues);
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData(true, "a.c", "a.c")]
        [InlineData(true, "a", "a.a")]
        [InlineData(false, "c", "a.c")]
        [InlineData(false, "a.c", "c")]
        [InlineData(false, "", "a.c")]
        [InlineData(false, "a.c", "")]
        [InlineData(true, "", "")]
        public void Test_SetEquals(bool excepted, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance();
            col.AddAll(values);

            var actual = col.SetEquals(otherValues);
            Assert.Equal(excepted, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            var stdResult = std.SetEquals(otherValues);
            Assert.Equal(stdResult, actual);
        }

        [Fact]
        public void Test_SetEquals_if_null()
        {
            var col = NewInstance();
            Assert.Throws<ArgumentNullException>(() => col.SetEquals(null));
        }

        [Fact]
        public void Test_SetEquals_if_equals_ref()
        {
            var values = new string[] { "a", "c", "e" };
            var col = NewInstance();
            col.AddAll(values);

            var actual = col.SetEquals(col);
            Assert.True(actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            var stdResult = std.SetEquals(std);
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData(true, "a", "A")]
        [InlineData(true, "A", "a")]
        public void Test_SetEquals_equalityComparer(bool excepted, string srcValues, string srcOtherValues)
        {
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string>(new IgnoreCaseStringComparer());
            col.AddAll(values);

            var actual = col.SetEquals(otherValues);
            Assert.Equal(excepted, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values, new IgnoreCaseStringComparer());
            var stdResult = std.SetEquals(otherValues);
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData("", "a.c", "a.c")]
        [InlineData("a", "c", "a.c")]
        [InlineData("a", "a.c", "c")]
        [InlineData("a.c", "", "a.c")]
        [InlineData("a.c", "a.c", "")]
        [InlineData("", "", "")]
        public void Test_SymmetricExceptWith(string srcExcepted, string srcValues, string srcOtherValues)
        {
            var expected = srcExcepted.Split('.').Where(v => v != "").ToArray();
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance();
            col.AddAll(values);

            col.SymmetricExceptWith(otherValues);
            var actual = col.ToArray();
            Assert.Equal(expected, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            std.SymmetricExceptWith(otherValues);
            var stdResult = std.ToArray();
            Assert.Equal(stdResult, actual);
        }

        [Fact]
        public void Test_SymmetricExceptWith_if_null()
        {
            var col = NewInstance();
            Assert.Throws<ArgumentNullException>(() => col.SymmetricExceptWith(null));
        }

        [Fact]
        public void Test_SymmetricExceptWith_if_equals_ref()
        {
            var values = new string[] { "a", "c", "e" };
            var col = NewInstance();
            col.AddAll(values);

            col.SymmetricExceptWith(col);
            var actual = col.ToArray();
            Assert.Equal(new string[] { }, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            std.SymmetricExceptWith(std);
            var stdResult = std.ToArray();
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData("", "a", "A")]
        [InlineData("", "A", "a")]
        public void Test_SymmetricExceptWith_with_equalityComparer(string srcExcepted, string srcValues, string srcOtherValues)
        {
            var expected = srcExcepted.Split('.').Where(v => v != "").ToArray();
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string>(new IgnoreCaseStringComparer());
            col.AddAll(values);

            col.SymmetricExceptWith(otherValues);
            var actual = col.ToArray();
            Assert.Equal(expected, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values, new IgnoreCaseStringComparer());
            std.SymmetricExceptWith(otherValues);
            var stdResult = std.ToArray();
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData("a.c", "a.c", "a.c")]
        [InlineData("c.a", "c", "a.c")]
        [InlineData("a.c", "a.c", "c")]
        [InlineData("a.c", "", "a.c")]
        [InlineData("a.c", "a.c", "")]
        [InlineData("", "", "")]
        public void Test_UnionWith(string srcExcepted, string srcValues, string srcOtherValues)
        {
            var expected = srcExcepted.Split('.').Where(v => v != "").ToArray();
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance();
            col.AddAll(values);

            col.UnionWith(otherValues);
            var actual = col.ToArray();
            Assert.Equal(expected, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            std.UnionWith(otherValues);
            var stdResult = std.ToArray();
            Assert.Equal(stdResult, actual);
        }

        [Fact]
        public void Test_UnionWith_if_null()
        {
            var col = NewInstance();
            Assert.Throws<ArgumentNullException>(() => col.UnionWith(null));
        }

        [Fact]
        public void Test_UnionWith_if_equals_ref()
        {
            var values = new string[] { "a", "c", "e" };
            var col = NewInstance();
            col.AddAll(values);

            col.UnionWith(col);
            var actual = col.ToArray();
            Assert.Equal(new string[] { "a", "c", "e" }, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values);
            std.UnionWith(std);
            var stdResult = std.ToArray();
            Assert.Equal(stdResult, actual);
        }

        [Theory]
        [InlineData("a", "a", "A")]
        [InlineData("A", "A", "a")]
        public void Test_UnionWith_with_equalityComparer(string srcExcepted, string srcValues, string srcOtherValues)
        {
            var expected = srcExcepted.Split('.').Where(v => v != "").ToArray();
            var values = srcValues.Split('.').Where(v => v != "");
            var otherValues = srcOtherValues.Split('.').Where(v => v != "");

            var col = NewInstance<string>(new IgnoreCaseStringComparer());
            col.AddAll(values);

            col.UnionWith(otherValues);
            var actual = col.ToArray();
            Assert.Equal(expected, actual);

            // StandardClass compatibility
            var std = new HashSet<string>(values, new IgnoreCaseStringComparer());
            std.UnionWith(otherValues);
            var stdResult = std.ToArray();
            Assert.Equal(stdResult, actual);
        }
    }
}