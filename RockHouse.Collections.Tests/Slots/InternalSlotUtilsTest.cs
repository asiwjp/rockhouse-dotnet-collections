using RockHouse.Collections.Slots;
using System;
using Xunit;

namespace Tests.Slots
{
    public class InternalSlotUtilsTest
    {
        [Fact]
        public void Test_Equals()
        {
            var slot1 = new Slot<int>(1);
            var slot2 = new Slot<int>(1);

            Assert.True(InternalSlotUtils.Equals(slot1, slot2));
        }

        [Fact]
        public void Test_Equals_null_both()
        {
            var slot1 = null as Slot<string>;
            var slot2 = null as Slot<string>;

            Assert.True(InternalSlotUtils.Equals(slot1, slot2));
        }

        [Fact]
        public void Test_Equals_null_left()
        {
            var slot1 = null as Slot<string>;
            var slot2 = new Slot<string>("a");

            Assert.False(InternalSlotUtils.Equals(slot1, slot2));
        }

        [Fact]
        public void Test_Equals_null_right()
        {
            var slot1 = new Slot<string>("a");
            var slot2 = null as Slot<string>;

            Assert.False(InternalSlotUtils.Equals(slot1, slot2));
        }

        [Theory]
        [InlineData(true, null, null)]
        [InlineData(false, "a", null)]
        [InlineData(false, null, "a")]
        public void Test_Equals_null_value(bool expected, string s1Item1, string s2Item1)
        {
            var slot1 = new Slot<string>(s1Item1);
            var slot2 = new Slot<string>(s2Item1);

            Assert.Equal(expected, InternalSlotUtils.Equals(slot1, slot2));
        }

        [Fact]
        public void Test_Equals_ref_equals()
        {
            var slot = new Slot<int>(1);

            Assert.True(InternalSlotUtils.Equals(slot, slot));
        }

        [Fact]
        public void Test_Equals_notEquals_value()
        {
            var slot1 = new Slot<string>("a");
            var slot2 = new Slot<string>("b");

            Assert.False(InternalSlotUtils.Equals(slot1, slot2));
        }

        [Fact]
        public void Test_Equals_notEquals_length()
        {
            var slot1 = new Slot<string>("a");
            var slot2 = new Slot<string, string>("a", "a");

            Assert.False(InternalSlotUtils.Equals(slot1, slot2));
        }

        [Fact]
        public void Test_Equals_notEquals_type()
        {
            var slot1 = new Slot<int>(1);
            var slot2 = 1;

            Assert.False(InternalSlotUtils.Equals(slot1, slot2));
        }

        [Fact]
        public void Test_Equals_notEquals_itemType()
        {
            var slot1 = new Slot<int>(1);
            var slot2 = new Slot<double>(1);

            Assert.False(InternalSlotUtils.Equals(slot1, slot2));
        }

        [Fact]
        public void Test_GetHashCode()
        {
            var slot = new Slot<int>(1);

            var actual = InternalSlotUtils.GetHashCode(slot);

            Assert.Equal(1.GetHashCode(), actual);
        }

        [Fact]
        public void Test_GetHashCode_length_gt_1()
        {
            var slot = new Slot<int, int>(1, 2);

            var actual = InternalSlotUtils.GetHashCode(slot);

            Assert.Equal(HashCode.Combine(1, 2), actual);
        }

        [Fact]
        public void Test_GetHashCode_null()
        {
            ISlot? slot = null;

            Assert.Throws<ArgumentNullException>(() => InternalSlotUtils.GetHashCode(slot));
        }

        [Fact]
        public void Test_GetHashCode_null_item()
        {
            var slot = new Slot<string?>(null as string);

            Assert.Equal(0, InternalSlotUtils.GetHashCode(slot));
        }

        [Fact]
        public void Test_GetHashCode_null_items()
        {
            var slot = new Slot<string?, string?>(null as string, null as string);

            Assert.Equal(HashCode.Combine<string?, string?>(null, null), InternalSlotUtils.GetHashCode(slot));
        }

        [Theory]
        [InlineData(0, 1, 1)]
        [InlineData(-1, 0, 1)]
        [InlineData(1, 1, 0)]
        public void Test_CompareTo(int expected, int s1Item1, int s2Item1)
        {
            var slot1 = new Slot<int>(s1Item1);
            var slot2 = new Slot<int>(s2Item1);

            var actual = InternalSlotUtils.CompareTo(slot1, slot2);

            Assert.Equal(expected, actual);

            // Tuple compatibility
            var t1 = slot1.ToTuple() as IComparable;
            var t2 = slot2.ToTuple() as IComparable;
            Assert.Equal(t1.CompareTo(t2), actual);
        }

        [Fact]
        public void Test_CompareTo_if_notEqualLength()
        {
            var slot1 = new Slot<int>(1);
            var slot2 = new Slot<int, int>(1, 2);
            Assert.Throws<ArgumentException>(() => InternalSlotUtils.CompareTo(slot1, slot2));

            // Tuple compatibility
            var t1 = slot1.ToTuple() as IComparable;
            var t2 = slot2.ToTuple() as IComparable;
            //Assert.Equal(t1.CompareTo(t2), actual);
            Assert.Throws<ArgumentException>(() => t1.CompareTo(t2));
        }

        [Fact]
        public void Test_CompareTo_null_both()
        {
            var slot1 = null as Slot<int>;
            var slot2 = null as Slot<int>;

            var actual = InternalSlotUtils.CompareTo(slot1, slot2);

            Assert.Equal(0, actual);
        }

        [Fact]
        public void Test_CompareTo_null_left()
        {
            var slot1 = null as Slot<int>;
            var slot2 = new Slot<int>(1);

            var actual = InternalSlotUtils.CompareTo(slot1, slot2);

            Assert.Equal(-1, actual);
        }

        [Fact]
        public void Test_CompareTo_null_right()
        {
            var slot1 = new Slot<int>(1);
            var slot2 = null as Slot<int>;

            var actual = InternalSlotUtils.CompareTo(slot1, slot2);

            Assert.Equal(1, actual);

            // Tuple compatibility
            var t1 = slot1.ToTuple() as IComparable;
            var t2 = null as IComparable;
            Assert.Equal(t1.CompareTo(t2), actual);
        }

        [Theory]
        [InlineData(0, null, null)]
        [InlineData(1, 1, null)]
        [InlineData(-1, null, 1)]
        public void Test_CompareTo_null_item(int expected, int? s1Item1, int? s2Item1)
        {
            var slot1 = new Slot<int?>(s1Item1);
            var slot2 = new Slot<int?>(s2Item1);

            var actual = InternalSlotUtils.CompareTo(slot1, slot2);

            Assert.Equal(expected, actual);

            // Tuple compatibility
            var t1 = slot1.ToTuple() as IComparable;
            var t2 = slot2.ToTuple();
            Assert.Equal(t1.CompareTo(t2), actual);
        }

        [Fact]
        public void Test_ToString()
        {
            var slot1 = new Slot<int>(1);
            var actual = InternalSlotUtils.ToString(slot1);
            Assert.Equal("(1)", actual);

            // Tuple compatibility
            var t1 = slot1.ToTuple();
            Assert.Equal(t1.ToString(), actual);
        }

        [Fact]
        public void Test_ToString_null()
        {
            var actual = InternalSlotUtils.ToString(null);
            Assert.Equal("", actual);
        }
    }
}