using RockHouse.Collections.Slots;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Slots
{
    public class Slot2Test
    {
        [Fact]
        public void Test___ctor_default()
        {
            var slot = new Slot<int, int>();
            Assert.Equal(0, slot.Item1);
            Assert.Equal(0, slot.Item2);
            Assert.Equal(0, slot.Count);
            Assert.True(slot.IsEmpty);
        }

        [Fact]
        public void Test___ctor_default_nullable()
        {
            var slot = new Slot<int?, int?>();
            Assert.Null(slot.Item1);
            Assert.Null(slot.Item2);
            Assert.Equal(0, slot.Count);
            Assert.True(slot.IsEmpty);
        }

        [Fact]
        public void Test___ctor_with_value()
        {
            var slot = new Slot<string, string>("a", "b");
            Assert.Equal("a", slot.Item1);
            Assert.Equal("b", slot.Item2);
            Assert.Equal(2, slot.Count);
            Assert.False(slot.IsEmpty);
        }

        [Fact]
        public void Test___ctor_with_null()
        {
            var slot = new Slot<string, string>(null as string, null as string);
            Assert.Null(slot.Item1);
            Assert.Null(slot.Item2);
            Assert.Equal(2, slot.Count);
            Assert.False(slot.IsEmpty);
        }

        [Fact]
        public void Test___ctor_with_slot()
        {
            var slot = new Slot<string, string>(new Slot<string, string>("a", "b"));
            Assert.Equal("a", slot.Item1);
            Assert.Equal("b", slot.Item2);
            Assert.Equal(2, slot.Count);
            Assert.False(slot.IsEmpty);
        }

        [Fact]
        public void Test___ctor_with_slot_if_empty()
        {
            var slot = new Slot<string, string>(new Slot<string, string>());
            Assert.Null(slot.Item1);
            Assert.Null(slot.Item2);
            Assert.Equal(0, slot.Count);
            Assert.True(slot.IsEmpty);
        }

        [Fact]
        public void Test___ctor_with_tuple()
        {
            var slot = new Slot<string, string>(new Tuple<string, string>("a", "b"));
            Assert.Equal("a", slot.Item1);
            Assert.Equal("b", slot.Item2);
            Assert.Equal(2, slot.Count);
            Assert.False(slot.IsEmpty);
        }

        [Fact]
        public void Test___ctor_with_valueTuple()
        {
            var slot = new Slot<string, string>(new ValueTuple<string, string>("a", "b"));
            Assert.Equal("a", slot.Item1);
            Assert.Equal("b", slot.Item2);
            Assert.Equal(2, slot.Count);
            Assert.False(slot.IsEmpty);
        }

        [Fact]
        public void Test___indexer()
        {
            var slot = new Slot<int, int>(11, 12);
            Assert.Throws<ArgumentOutOfRangeException>(() => slot[-1]);
            Assert.Equal(11, slot[0]);
            Assert.Equal(12, slot[1]);
            Assert.Throws<ArgumentOutOfRangeException>(() => slot[slot.Length]);
        }

        [Theory]
        [InlineData("a", 1)]
        [InlineData(null, null)]
        public void Test___prop_Items(string item1, int? item2)
        {
            var slot = new Slot<string, int?>();
            slot.Item1 = item1;
            Assert.Equal(1, slot.Count);
            slot.Item2 = item2;
            Assert.Equal(2, slot.Count);

            Assert.Equal(item1, slot.Item1);
            Assert.Equal(item1, slot[0]);
            Assert.False(slot.IsEmpty);

            Assert.Equal(item2, slot.Item2);
            Assert.Equal(item2, slot[1]);
            Assert.False(slot.IsEmpty);
        }

        [Fact]
        public void Test__ITuple__prop_Length()
        {
            Assert.Equal(2, new Slot<string, string>().Length);
        }

        [Fact]
        public void Test__IComparable_CompareTo()
        {
            var slots = new List<Slot<int?, int?>>()
            {
                new Slot<int?, int?>(1, 1),
                new Slot<int?, int?>(1, 0),
                new Slot<int?, int?>(0, 1),
                new Slot<int?, int?>(0, 0),
                new Slot<int?, int?>(0, -1),
                new Slot<int?, int?>(-1, 0),
                new Slot<int?, int?>(-1, -1),
                new Slot<int?, int?>(1, null),
                new Slot<int?, int?>(null, 1),
                new Slot<int?, int?>(null, null)
            };
            var tuples = slots.Select(s => s.ToTuple()).ToList();

            slots.Sort();
            var actuals = slots.ToArray();

            var i = 0;
            Assert.Equal("(, )", actuals[i++].ToString());
            Assert.Equal("(, 1)", actuals[i++].ToString());
            Assert.Equal("(-1, -1)", actuals[i++].ToString());
            Assert.Equal("(-1, 0)", actuals[i++].ToString());
            Assert.Equal("(0, -1)", actuals[i++].ToString());
            Assert.Equal("(0, 0)", actuals[i++].ToString());
            Assert.Equal("(0, 1)", actuals[i++].ToString());
            Assert.Equal("(1, )", actuals[i++].ToString());
            Assert.Equal("(1, 0)", actuals[i++].ToString());
            Assert.Equal("(1, 1)", actuals[i++].ToString());

            // Tuple compatibility
            var actualTuples = slots.Select(s => s.ToTuple()).ToArray();

            tuples.Sort();
            var expectedTuples = tuples.ToArray();
            Assert.Equal(expectedTuples, actualTuples);
        }

        [Fact]
        public void Test__ISlot_IsFree()
        {
            var slot = new Slot<int, int>();

            Assert.True(slot.IsFree(0));
            slot.Item1 = 1;
            Assert.False(slot.IsFree(0));

            Assert.True(slot.IsFree(1));
            slot.Item2 = 1;
            Assert.False(slot.IsFree(1));
        }

        [Fact]
        public void Test__ISlot_IsFree_if_out_of_range()
        {
            var slot = new Slot<int, int>();

            Assert.Throws<ArgumentOutOfRangeException>(() => slot.IsFree(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => slot.IsFree(slot.Length + 1));
        }
        [Fact]
        public void Test__ISlot_Set()
        {
            var slot = new Slot<string, int>();
            slot.Set(0, "a");
            slot.Set(1, 1);
        }

        [Fact]
        public void Test__ISlot_Set_if_out_of_range()
        {
            var slot = new Slot<string, int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => slot.Set(-1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => slot.Set(slot.Length, 1));
        }

        [Fact]
        public void Test__Object_Equals()
        {
            var slot1 = new Slot<string, string>("a", "b");
            var slot2 = new Slot<string, string>("a", "b");

            var actual = slot1.Equals(slot2);

            Assert.True(actual);
        }

        [Fact]
        public void Test__Object_Equals_notEquals_value()
        {
            var slot1 = new Slot<string, string>("a", "a");
            var slot2 = new Slot<string, string>("a", "b");

            var actual = slot1.Equals(slot2);

            Assert.False(actual);
        }

        [Fact]
        public void Test__Object_Equals_notEquals_length()
        {
            var slot1 = new Slot<string, string>("a", "a");
            var slot2 = new Slot<string>("a");

            var actual = slot1.Equals(slot2);

            Assert.False(actual);
        }

        [Fact]
        public void Test__Object_Equals_notEquals_type()
        {
            var slot1 = new Slot<int, int>(1, 2);
            var slot2 = new Slot<double, int>(1, 2);

            var actual = slot1.Equals(slot2);

            Assert.False(actual);
        }

        [Fact]
        public void Test__Object_Equals_null()
        {
            var slot = new Slot<int, int>(1, 2);

            var actual = slot.Equals(null);

            Assert.False(actual);
        }

        [Theory]
        [InlineData(true, null, null, null, null)]
        [InlineData(false, 1, null, null, null)]
        [InlineData(false, null, 1, null, null)]
        public void Test__Object_Equals_null_value(bool expected, int? s1Item1, int? s1Item2, int? s2Item1, int? s2Item2)
        {
            var slot1 = new Slot<int?, int?>(s1Item1, s1Item2);
            var slot2 = new Slot<int?, int?>(s2Item1, s2Item2);

            var actual = slot1.Equals(slot2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test__Object_GetHashCode()
        {
            var slot = new Slot<string, string>("a", "b");
            var expected = HashCode.Combine("a", "b");
            Assert.Equal(expected, slot.GetHashCode());
        }

        [Fact]
        public void Test__Object_ToString()
        {
            var actual = new Slot<string, string>("a", "b").ToString();
            var expected = "(a, b)";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_RemoveAll()
        {
            var expected = ValueTuple.Create("a", "b");
            var slot = new Slot<string, string>(expected);
            var actualRes = slot.RemoveAll();

            Assert.Equal(expected, actualRes);
            Assert.True(slot.IsEmpty);
        }

        [Fact]
        public void Test_ToTuple()
        {
            var actual = new Slot<string, string>("a", "b");
            var expected = Tuple.Create("a", "b");
            Assert.Equal(expected, actual.ToTuple());
        }

        [Fact]
        public void Test_ToTuple_with_nullValue()
        {
            var actual = new Slot<string?, string?>(null as string, null as string);
            var expected = Tuple.Create(null as string, null as string);
            Assert.Equal(expected, actual.ToTuple());
        }

        [Fact]
        public void Test_ToValueTuple()
        {
            var actual = new Slot<string, string>("a", "b");
            var expected = ValueTuple.Create("a", "b");
            Assert.Equal(expected, actual.ToValueTuple());
        }
    }
}