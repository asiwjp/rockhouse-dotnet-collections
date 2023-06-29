using System;
using System.Collections.Generic;
using Xunit;

namespace RockHouse.Collections.Tests
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
    }
}
