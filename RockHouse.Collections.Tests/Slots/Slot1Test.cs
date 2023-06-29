using RockHouse.Collections.Slots;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Slots
{
    public class Slot1Test
    {
        [Fact]
        public void Test___ctor_default()
        {
            var slot = new Slot<int>();
            Assert.Equal(0, slot.Item1);
            Assert.Equal(0, slot.Count);
            Assert.True(slot.IsEmpty);
        }

        [Fact]
        public void Test___ctor_default_nullable()
        {
            var slot = new Slot<int?>();
            Assert.Null(slot.Item1);
            Assert.Equal(0, slot.Count);
            Assert.True(slot.IsEmpty);
        }

        [Fact]
        public void Test___ctor_with_value()
        {
            var slot = new Slot<string>("a");
            Assert.Equal("a", slot.Item1);
            Assert.Equal(1, slot.Count);
            Assert.False(slot.IsEmpty);
        }

        [Fact]
        public void Test___ctor_with_null()
        {
            var slot = new Slot<string>(null as string);
            Assert.Null(slot.Item1);
            Assert.Equal(1, slot.Count);
            Assert.False(slot.IsEmpty);
        }

        [Fact]
        public void Test___ctor_with_slot()
        {
            var slot = new Slot<string>(new Slot<string>("a"));
            Assert.Equal("a", slot.Item1);
            Assert.Equal(1, slot.Count);
            Assert.False(slot.IsEmpty);
        }

        [Fact]
        public void Test___ctor_with_slot_if_empty()
        {
            var slot = new Slot<string>(new Slot<string>());
            Assert.Null(slot.Item1);
            Assert.Equal(0, slot.Count);
            Assert.True(slot.IsEmpty);
        }

        [Fact]
        public void Test___ctor_with_tuple()
        {
            var slot = new Slot<string>(new Tuple<string>("a"));
            Assert.Equal("a", slot.Item1);
            Assert.Equal(1, slot.Count);
            Assert.False(slot.IsEmpty);
        }

        [Fact]
        public void Test___ctor_with_valueTuple()
        {
            var slot = new Slot<string>(new ValueTuple<string>("a"));
            Assert.Equal("a", slot.Item1);
            Assert.Equal(1, slot.Count);
            Assert.False(slot.IsEmpty);
        }

        [Fact]
        public void Test___indexer()
        {
            var slot = new Slot<int>(11);
            Assert.Throws<ArgumentOutOfRangeException>(() => slot[-1]);
            Assert.Equal(11, slot[0]);
            Assert.Throws<ArgumentOutOfRangeException>(() => slot[1]);
        }

        [Theory]
        [InlineData("a")]
        [InlineData(null)]
        public void Test___prop_Items(string item1)
        {
            var slot = new Slot<string>();
            slot.Item1 = item1;
            Assert.Equal(1, slot.Count);
            Assert.False(slot.IsEmpty);

            Assert.Equal(item1, slot.Item1);
            Assert.Equal(item1, slot[0]);
        }


        [Fact]
        public void Test___prop_Length()
        {
            Assert.Equal(1, new Slot<string>().Length);
        }

        [Fact]
        public void Test__IComparable_CompareTo()
        {
            var slots = new List<Slot<int?>>()
            {
                new Slot<int?>(1),
                new Slot<int?>(0),
                new Slot<int?>(-1),
                new Slot<int?>(null as int?),
            };
            var tuples = slots.Select(s => s.ToTuple()).ToList();

            slots.Sort();
            var actuals = slots.Select(s => s.Item1).ToArray();

            Assert.Null(actuals[0]);
            Assert.Equal(-1, actuals[1]);
            Assert.Equal(0, actuals[2]);
            Assert.Equal(1, actuals[3]);

            // Tuple compatibility
            var actualTuples = slots.Select(s => s.ToTuple()).ToArray();

            tuples.Sort();
            var expectedTuples = tuples.ToArray();
            Assert.Equal(expectedTuples, actualTuples);
        }

        [Fact]
        public void Test__ISlot_IsFree()
        {
            var slot = new Slot<int>();
            Assert.True(slot.IsFree(0));
            slot.Item1 = 1;
            Assert.False(slot.IsFree(0));
        }

        [Fact]
        public void Test__ISlot_IsFree_if_out_of_range()
        {
            var slot = new Slot<int>();

            Assert.Throws<ArgumentOutOfRangeException>(() => slot.IsFree(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => slot.IsFree(slot.Length + 1));
        }

        [Fact]
        public void Test__ISlot_Set()
        {
            var slot = new Slot<string>();
            slot.Set(0, "a");
            Assert.Equal("a", slot.Item1);
        }

        [Fact]
        public void Test__ISlot_Set_if_out_of_range()
        {
            var slot = new Slot<string>();
            Assert.Throws<ArgumentOutOfRangeException>(() => slot.Set(-1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => slot.Set(slot.Length, 1));
        }

        [Fact]
        public void Test__Object_Equals()
        {
            var slot1 = new Slot<string>("a");
            var slot2 = new Slot<string>("a");

            var actual = slot1.Equals(slot2);

            Assert.True(actual);
        }

        [Fact]
        public void Test__Object_Equals_notEquals_value()
        {
            var slot1 = new Slot<string>("a");
            var slot2 = new Slot<string>("b");

            var actual = slot1.Equals(slot2);

            Assert.False(actual);
        }

        [Fact]
        public void Test__Object_Equals_notEquals_length()
        {
            var slot1 = new Slot<string>("a");
            var slot2 = new Slot<string, string>("a", "a");

            var actual = slot1.Equals(slot2);

            Assert.False(actual);
        }

        [Fact]
        public void Test__Object_Equals_notEquals_type()
        {
            var slot1 = new Slot<int>(1);
            var slot2 = new Slot<double>(1);

            var actual = slot1.Equals(slot2);

            Assert.False(actual);
        }

        [Fact]
        public void Test__Object_Equals_null()
        {
            var slot = new Slot<int>(1);

            var actual = slot.Equals(null);

            Assert.False(actual);

            // Tuple compatibility
            var t1 = slot.ToTuple();
            var t2 = null as Tuple<int>;
            Assert.Equal(t1.Equals(t2), actual);
        }

        [Theory]
        [InlineData(true, null, null)]
        [InlineData(false, 1, null)]
        [InlineData(false, null, 1)]
        public void Test__Object_Equals_null_value(bool expected, int? s1Item1, int? s2Item1)
        {
            var slot1 = new Slot<int?>(s1Item1);
            var slot2 = new Slot<int?>(s2Item1);

            var actual = slot1.Equals(slot2);

            Assert.Equal(expected, actual);

            // Tuple compatibility
            var t1 = slot1.ToTuple();
            var t2 = slot2.ToTuple();
            Assert.Equal(t1.Equals(t2), actual);
        }

        [Fact]
        public void Test__Object_GetHashCode()
        {
            var slot = new Slot<string>("a");
            Assert.Equal("a".GetHashCode(), slot.GetHashCode());
        }

        [Fact]
        public void Test__Object_ToString()
        {
            var actual = new Slot<string>("a").ToString();
            var expected = "(a)";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_RemoveAll()
        {
            var expected = ValueTuple.Create("a");
            var slot = new Slot<string>(expected);
            var actualRes = slot.RemoveAll();

            Assert.Equal(expected, actualRes);
            Assert.True(slot.IsEmpty);
        }

        [Fact]
        public void Test_ToTuple()
        {
            var actual = new Slot<string>("a");
            var expected = Tuple.Create("a");
            Assert.Equal(expected, actual.ToTuple());
        }

        [Fact]
        public void Test_ToTuple_with_nullValue()
        {
            var actual = new Slot<string?>(null as string);
            var expected = Tuple.Create(null as string);
            Assert.Equal(expected, actual.ToTuple());
        }

        [Fact]
        public void Test_ToValueTuple()
        {
            var actual = new Slot<string>("a");
            var expected = ValueTuple.Create("a");
            Assert.Equal(expected, actual.ToValueTuple());
        }
    }
}