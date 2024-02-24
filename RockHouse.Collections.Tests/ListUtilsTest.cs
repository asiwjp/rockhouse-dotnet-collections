using RockHouse.Collections;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class ListUtilsTest
    {
        [Fact]
        public void Test_Count()
        {
            var col = new List<int>
            {
                1
            };

            var actual = ListUtils.Count(col);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void Test_Count_if_null()
        {
            List<int> col = null;
            var actual = ListUtils.Count(col);
            Assert.Equal(0, actual);
        }

        [Fact]
        public void Test_DefaultIfEmpty__with_defaultValue()
        {
            var col = new List<int>
            {
            };

            var actual = ListUtils.DefaultIfEmpty(col, new List<int> { 0 });
            Assert.Contains(0, actual);
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_empty()
        {
            var col = new List<int>
            {
            };

            var actual = ListUtils.DefaultIfEmpty(col, () => new List<int> { 0 });
            Assert.Contains(0, actual);
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_factory_is_null()
        {
            List<int> col = null;

            Assert.Throws<ArgumentNullException>(() => ListUtils.DefaultIfEmpty(col, null as Func<IList<int>>));
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_null()
        {
            List<int> col = null;

            var actual = ListUtils.DefaultIfEmpty(col, () => new List<int> { 0 });
            Assert.Contains(0, actual);
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_notEmpty()
        {
            var col = new List<int>
            {
                1
            };

            var actual = ListUtils.DefaultIfEmpty(col, () => new List<int> { 0 });
            Assert.Contains(1, actual);
            Assert.DoesNotContain(0, actual);
        }

        [Fact]
        public void Test_DefaultIfNull__with_defaultValue()
        {
            List<int> col = null;

            var actual = ListUtils.DefaultIfNull(col, new List<int> { 0 });
            Assert.Contains(0, actual);
        }

        [Fact]
        public void Test_DefaultIfNull_if_empty()
        {
            var col = new List<int>
            {
            };

            var actual = ListUtils.DefaultIfNull(col, () => new List<int> { 0 });
            Assert.Empty(actual);
        }

        [Fact]
        public void Test_DefaultIfNull_if_factory_is_null()
        {
            List<int> col = null;

            Assert.Throws<ArgumentNullException>(() => ListUtils.DefaultIfNull(col, null as Func<IList<int>>));
        }

        [Fact]
        public void Test_DefaultIfNull_if_null()
        {
            List<int> col = null;

            var actual = ListUtils.DefaultIfNull(col, () => new List<int> { 0 });
            Assert.Contains(0, actual);
        }

        [Fact]
        public void Test_DefaultIfNull_if_notEmpty()
        {
            var col = new List<int>
            {
                1
            };

            var actual = ListUtils.DefaultIfNull(col, () => new List<int> { 0 });
            Assert.Contains(1, actual);
            Assert.DoesNotContain(0, actual);
        }

        [Fact]
        public void Test_EmptyIfNull()
        {
            var col = new List<int>
            {
            };

            var actual = ListUtils.EmptyIfNull(col);
            Assert.True(object.ReferenceEquals(col, actual));
        }

        [Fact]
        public void Test_EmptyIfNull_if_null()
        {
            List<int> col = null;

            var actual = ListUtils.EmptyIfNull(col);
            Assert.Empty(actual);
        }


        [Fact]
        public void Test_IsEmpty()
        {
            var col = new List<int>
            {
            };

            var actual = ListUtils.IsEmpty(col);
            Assert.True(actual);
        }

        [Fact]
        public void Test_IsEmpty_if_notEmpty()
        {
            var col = new List<int>
            {
                1
            };

            var actual = ListUtils.IsEmpty(col);
            Assert.False(actual);
        }

        [Fact]
        public void Test_IsEmpty_if_null()
        {
            List<int> col = null;

            var actual = ListUtils.IsEmpty(col);
            Assert.True(actual);
        }

        [Fact]
        public void Test_IsNotEmpty()
        {
            var col = new List<int>
            {
                1
            };

            var actual = ListUtils.IsNotEmpty(col);
            Assert.True(actual);
        }

        [Fact]
        public void Test_IsNotEmpty_if_empty()
        {
            var col = new List<int>
            {
            };

            var actual = ListUtils.IsNotEmpty(col);
            Assert.False(actual);
        }

        [Fact]
        public void Test_IsNotEmpty_if_null()
        {
            List<int> col = null;

            var actual = ListUtils.IsNotEmpty(col);
            Assert.False(actual);
        }

        [Fact]
        public void Test_Pop()
        {
            var col = new List<int> { 1, 2, 3 };
            var actual = ListUtils.Pop(col);
            Assert.Equal(3, actual);
            Assert.Equal(new int[] { 1, 2 }, col.ToArray());
        }

        [Fact]
        public void Test_Pop__if_empty()
        {
            var col = new List<int>();
            Assert.Throws<InvalidOperationException>(() => ListUtils.Pop(col));
        }

        [Fact]
        public void Test_Pop__if_invalid_index()
        {
            var col = new List<int> { 1 };
            Assert.Throws<ArgumentOutOfRangeException>(() => ListUtils.Pop(col, -2));
            Assert.Throws<ArgumentOutOfRangeException>(() => ListUtils.Pop(col, 1));
        }

        [Fact]
        public void Test_Pop__with_index()
        {
            var col = new List<int> { 1, 2, 3 };
            var actual = ListUtils.Pop(col, 1);
            Assert.Equal(2, actual);
            Assert.Equal(new int[] { 1, 3 }, col.ToArray());
        }

        [Fact]
        public void Test_PopOrDefault()
        {
            var col = new List<int> { 1, 2 };
            var actual = ListUtils.PopOrDefault(col);
            Assert.Equal(2, actual);
            Assert.Equal(new int[] { 1 }, col.ToArray());
        }

        [Fact]
        public void Test_PopOrDefault__if_defaultValueFactory_is_null()
        {
            var col = new List<int>();
            Assert.Throws<ArgumentNullException>(() => ListUtils.PopOrDefault(col, null));
        }

        [Fact]
        public void Test_PopOrDefault__if_empty()
        {
            var col = new List<int>();
            var actual = ListUtils.PopOrDefault(col);
            Assert.Equal(0, actual);
            Assert.Empty(col);
        }

        [Fact]
        public void Test_PopOrDefault__if_invalid_index()
        {
            var col = new List<int>();
            Assert.Throws<ArgumentNullException>(() => ListUtils.PopOrDefault(col, index: -2));
        }

        [Fact]
        public void Test_PopOrDefault__with_defaultValue()
        {
            var col = new List<int>();
            var actual = ListUtils.PopOrDefault(col, 1);
            Assert.Equal(1, actual);
            Assert.Empty(col);
        }

        [Fact]
        public void Test_PopOrDefault__with_index()
        {
            var col = new List<int> { 1, 2, 3 };
            var actual = ListUtils.PopOrDefault(col, index: 0);
            Assert.Equal(1, actual);
            Assert.Equal(new int[] { 2, 3 }, col);
        }

        [Fact]
        public void Test_PopOrDefault__with_defaultValueFactory()
        {
            var col = new List<int>();
            var actual = ListUtils.PopOrDefault(col, () => 1);
            Assert.Equal(1, actual);
            Assert.Empty(col);
        }

        [Fact]
        public void Test_Push()
        {
            var col = new List<int> { 1 };
            ListUtils.Push(col, 2, 3);
            Assert.Equal(new int[] { 1, 2, 3 }, col.ToArray());
        }

        [Fact]
        public void Test_Shift()
        {
            var col = new List<int> { 1, 2, 3 };
            var actual = ListUtils.Shift(col);

            Assert.Equal(1, actual);
            Assert.Equal(new int[] { 2, 3 }, col.ToArray());
        }

        [Fact]
        public void Test_Shift__if_empty()
        {
            var col = new List<int>();
            Assert.Throws<InvalidOperationException>(() => ListUtils.Shift(col));
        }

        [Fact]
        public void Test_ShiftOrDefault()
        {
            var col = new List<int> { 1, 2, 3 };
            var actual = ListUtils.ShiftOrDefault(col);
            Assert.Equal(1, actual);
            Assert.Equal(new int[] { 2, 3 }, col.ToArray());
        }

        [Fact]
        public void Test_ShiftOrDefault__if_empty()
        {
            var col = new List<int>();
            var actual = ListUtils.ShiftOrDefault(col);
            Assert.Equal(0, actual);
        }

        [Fact]
        public void Test_ShiftOrDefault__with_defaultValue()
        {
            var col = new List<int>();
            var actual = ListUtils.ShiftOrDefault(col, 1);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void Test_ShiftOrDefault__with_defaultValueFactory()
        {
            var col = new List<int>();
            var actual = ListUtils.ShiftOrDefault(col, () => 1);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void Test_Unshift()
        {
            var col = new List<int> { 3 };
            ListUtils.Unshift(col, 1, 2);

            Assert.Equal(new int[] { 1, 2, 3 }, col.ToArray());
        }
    }
}