using System;
using System.Collections.Generic;
using Xunit;

namespace RockHouse.Collections.Tests
{
    public class SetUtilsTest
    {
        [Fact]
        public void Test_Count()
        {
            var col = new HashSet<int>
            {
                1
            };

            var actual = SetUtils.Count(col);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void Test_Count_if_null()
        {
            HashSet<int> col = null;
            var actual = SetUtils.Count(col);
            Assert.Equal(0, actual);
        }

        [Fact]
        public void Test_DefaultIfEmpty__with_defaultValue()
        {
            var col = new HashSet<int>
            {
            };

            var actual = SetUtils.DefaultIfEmpty(col, new HashSet<int> { 0 });
            Assert.Contains(0, actual);
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_empty()
        {
            var col = new HashSet<int>
            {
            };

            var actual = SetUtils.DefaultIfEmpty(col, () => new HashSet<int> { 0 });
            Assert.Contains(0, actual);
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_factory_is_null()
        {
            HashSet<int> col = null;

            Assert.Throws<ArgumentNullException>(() => SetUtils.DefaultIfEmpty(col, null as Func<ISet<int>>));
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_null()
        {
            HashSet<int> col = null;

            var actual = SetUtils.DefaultIfEmpty(col, () => new HashSet<int> { 0 });
            Assert.Contains(0, actual);
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_notEmpty()
        {
            var col = new HashSet<int>
            {
                1
            };

            var actual = SetUtils.DefaultIfEmpty(col, () => new HashSet<int> { 0 });
            Assert.Contains(1, actual);
            Assert.DoesNotContain(0, actual);
        }

        [Fact]
        public void Test_DefaultIfNull__with_defaultValue()
        {
            HashSet<int> col = null;

            var actual = SetUtils.DefaultIfNull(col, new HashSet<int> { 0 });
            Assert.Contains(0, actual);
        }

        [Fact]
        public void Test_DefaultIfNull_if_empty()
        {
            var col = new HashSet<int>
            {
            };

            var actual = SetUtils.DefaultIfNull(col, () => new HashSet<int> { 0 });
            Assert.Empty(actual);
        }

        [Fact]
        public void Test_DefaultIfNull_if_factory_is_null()
        {
            HashSet<int> col = null;

            Assert.Throws<ArgumentNullException>(() => SetUtils.DefaultIfNull(col, null as Func<ISet<int>>));
        }

        [Fact]
        public void Test_DefaultIfNull_if_null()
        {
            HashSet<int> col = null;

            var actual = SetUtils.DefaultIfNull(col, () => new HashSet<int> { 0 });
            Assert.Contains(0, actual);
        }

        [Fact]
        public void Test_DefaultIfNull_if_notEmpty()
        {
            var col = new HashSet<int>
            {
                1
            };

            var actual = SetUtils.DefaultIfNull(col, () => new HashSet<int> { 0 });
            Assert.Contains(1, actual);
            Assert.DoesNotContain(0, actual);
        }

        [Fact]
        public void Test_EmptyIfNull()
        {
            var col = new HashSet<int>
            {
            };

            var actual = SetUtils.EmptyIfNull(col);
            Assert.True(object.ReferenceEquals(col, actual));
        }

        [Fact]
        public void Test_EmptyIfNull_if_null()
        {
            HashSet<int> col = null;

            var actual = SetUtils.EmptyIfNull(col);
            Assert.Empty(actual);
        }


        [Fact]
        public void Test_IsEmpty()
        {
            var col = new HashSet<int>
            {
            };

            var actual = SetUtils.IsEmpty(col);
            Assert.True(actual);
        }

        [Fact]
        public void Test_IsEmpty_if_notEmpty()
        {
            var col = new HashSet<int>
            {
                1
            };

            var actual = SetUtils.IsEmpty(col);
            Assert.False(actual);
        }

        [Fact]
        public void Test_IsEmpty_if_null()
        {
            HashSet<int> col = null;

            var actual = SetUtils.IsEmpty(col);
            Assert.True(actual);
        }

        [Fact]
        public void Test_IsNotEmpty()
        {
            var col = new HashSet<int>
            {
                1
            };

            var actual = SetUtils.IsNotEmpty(col);
            Assert.True(actual);
        }

        [Fact]
        public void Test_IsNotEmpty_if_empty()
        {
            var col = new HashSet<int>
            {
            };

            var actual = SetUtils.IsNotEmpty(col);
            Assert.False(actual);
        }

        [Fact]
        public void Test_IsNotEmpty_if_null()
        {
            HashSet<int> col = null;

            var actual = SetUtils.IsNotEmpty(col);
            Assert.False(actual);
        }
    }
}
