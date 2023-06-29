using RockHouse.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class EnumerableUtilsTest
    {
        [Fact]
        public void Test_Count()
        {
            var col = new List<int>
            {
                1
            };

            var actual = EnumerableUtils.Count(col);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void Test_Count_if_need_enumeration()
        {
            var col = Enumerable.Range(1, 10);
            var actual = EnumerableUtils.Count(col);
            Assert.Equal(10, actual);
        }

        [Fact]
        public void Test_Count_if_null()
        {
            List<int> col = null;
            var actual = EnumerableUtils.Count(col);
            Assert.Equal(0, actual);
        }

        [Fact]
        public void Test_DefaultIfEmpty__with_defaultValue()
        {
            var col = new List<int>
            {
            };

            var actual = EnumerableUtils.DefaultIfEmpty(col, new int[] { 0 });
            Assert.Equal(new int[] { 0 }, actual);
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_empty()
        {
            var col = new List<int>
            {
            };

            var actual = EnumerableUtils.DefaultIfEmpty(col, () => new int[] { 0 });
            Assert.Equal(new int[] { 0 }, actual);
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_factory_is_null()
        {
            List<int> col = null;

            Assert.Throws<ArgumentNullException>(() => EnumerableUtils.DefaultIfEmpty(col, null as Func<IEnumerable<int>>));
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_null()
        {
            List<int> col = null;

            var actual = EnumerableUtils.DefaultIfEmpty(col, () => new int[] { 0 });
            Assert.Equal(new int[] { 0 }, actual);
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_notEmpty()
        {
            var col = new List<int>
            {
                1
            };

            var actual = EnumerableUtils.DefaultIfEmpty(col, () => new int[] { 0 });
            Assert.Equal(new int[] { 1 }, actual);
        }

        [Fact]
        public void Test_DefaultIfNull__with_defaultValue()
        {
            List<int> col = null;

            var actual = EnumerableUtils.DefaultIfNull(col, new int[] { 0 });
            Assert.Equal(new int[] { 0 }, actual);
        }

        [Fact]
        public void Test_DefaultIfNull_if_empty()
        {
            var col = new List<int>
            {
            };

            var actual = EnumerableUtils.DefaultIfNull(col, () => new int[] { 0 });
            Assert.Empty(actual);
        }

        [Fact]
        public void Test_DefaultIfNull_if_factory_is_null()
        {
            List<int> col = null;

            Assert.Throws<ArgumentNullException>(() => EnumerableUtils.DefaultIfNull(col, null as Func<IEnumerable<int>>));
        }

        [Fact]
        public void Test_DefaultIfNull_if_null()
        {
            List<int> col = null;

            var actual = EnumerableUtils.DefaultIfNull(col, () => new int[] { 0 });
            Assert.Equal(new int[] { 0 }, actual);
        }

        [Fact]
        public void Test_DefaultIfNull_if_notEmpty()
        {
            var col = new List<int>
            {
                1
            };

            var actual = EnumerableUtils.DefaultIfNull(col, () => new int[] { 0 });
            Assert.Equal(new int[] { 1 }, actual);
        }

        [Fact]
        public void Test_EmptyIfNull()
        {
            var col = new List<int>
            {
            };

            var actual = EnumerableUtils.EmptyIfNull(col);
            Assert.True(object.ReferenceEquals(col, actual));
        }

        [Fact]
        public void Test_EmptyIfNull_if_null()
        {
            List<int> col = null;

            var actual = EnumerableUtils.EmptyIfNull(col);
            Assert.Empty(actual);
        }

        [Fact]
        public void Test_IsEmpty()
        {
            var col = new List<int>
            {
            };

            var actual = EnumerableUtils.IsEmpty(col);
            Assert.True(actual);
        }

        [Fact]
        public void Test_IsEmpty_if_need_enumeration()
        {
            var col = Enumerable.Range(0, 0);

            var actual = EnumerableUtils.IsEmpty(col);
            Assert.False(actual);
        }

        [Fact]
        public void Test_IsEmpty_if_notEmpty()
        {
            var col = new List<int>
            {
                1
            };

            var actual = EnumerableUtils.IsEmpty(col);
            Assert.False(actual);
        }

        [Fact]
        public void Test_IsEmpty_if_null()
        {
            List<int> col = null;
            var actual = EnumerableUtils.IsEmpty(col);
            Assert.True(actual);
        }

        [Fact]
        public void Test_IsNotEmpty()
        {
            var col = new List<int>
            {
                1
            };

            var actual = EnumerableUtils.IsNotEmpty(col);
            Assert.True(actual);
        }

        [Fact]
        public void Test_IsNotEmpty_if_empty()
        {
            var col = new List<int>
            {
            };

            var actual = EnumerableUtils.IsNotEmpty(col);
            Assert.False(actual);
        }

        [Fact]
        public void Test_IsNotEmpty_if_null()
        {
            List<int> col = null;
            var actual = EnumerableUtils.IsNotEmpty(col);
            Assert.False(actual);
        }

        [Fact]
        public void Test_TryGetNonEnumeratedCount()
        {
            var col = new List<int>
            {
                1
            };

            var actualRet = EnumerableUtils.TryGetNonEnumeratedCount(col, out var actualCount);
            Assert.True(actualRet);
            Assert.Equal(1, actualCount);
        }

        [Fact]
        public void Test_TryGetNonEnumeratedCount_if_need_enumeration()
        {
            var col = Enumerable.Range(0, 10);
            var actualRet = EnumerableUtils.TryGetNonEnumeratedCount(col, out var actualCount);
            Assert.False(actualRet);
            Assert.Equal(0, actualCount);
        }

        [Fact]
        public void Test_TryGetNonEnumeratedCount_if_null()
        {
            List<int> col = null;
            var actualRet = EnumerableUtils.TryGetNonEnumeratedCount(col, out var actualCount);
            Assert.True(actualRet);
            Assert.Equal(0, actualCount);
        }

        [Fact]
        public void Test_TryGetNonEnumeratedCount_if_string()
        {
            string col = "a";
            var actualRet = EnumerableUtils.TryGetNonEnumeratedCount(col, out var actualCount);
            Assert.True(actualRet);
            Assert.Equal(1, actualCount);
        }
    }
}
