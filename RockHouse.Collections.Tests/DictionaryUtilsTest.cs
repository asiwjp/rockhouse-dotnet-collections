using System;
using System.Collections.Generic;
using Xunit;

namespace RockHouse.Collections.Tests
{
    public class DictionaryUtilsTest
    {
        [Fact]
        public void Test_Count()
        {
            var col = new Dictionary<int, int>
            {
                { 1, 1 }
            };

            var actual = DictionaryUtils.Count(col);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void Test_Count_if_null()
        {
            Dictionary<int, int> col = null;
            var actual = DictionaryUtils.Count(col);
            Assert.Equal(0, actual);
        }

        [Fact]
        public void Test_DefaultIfEmpty__with_defaultValue()
        {
            var col = new Dictionary<int, int>
            {
            };

            var actual = DictionaryUtils.DefaultIfEmpty(col, new Dictionary<int, int> { { 0, 0 } });
            Assert.Single(actual);
            Assert.Equal(0, actual[0]);
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_empty()
        {
            var col = new Dictionary<int, int>
            {
            };

            var actual = DictionaryUtils.DefaultIfEmpty(col, () => new Dictionary<int, int> { { 0, 0 } });
            Assert.Single(actual);
            Assert.Equal(0, actual[0]);
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_factory_is_null()
        {
            Dictionary<int, int> col = null;

            Assert.Throws<ArgumentNullException>(() => DictionaryUtils.DefaultIfEmpty(col, null as Func<IDictionary<int, int>>));
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_null()
        {
            Dictionary<int, int> col = null;

            var actual = DictionaryUtils.DefaultIfEmpty(col, () => new Dictionary<int, int> { { 0, 0 } });
            Assert.Single(actual);
            Assert.Equal(0, actual[0]);
        }

        [Fact]
        public void Test_DefaultIfEmpty_if_notEmpty()
        {
            var col = new Dictionary<int, int>
            {
                { 1, 1 }
            };

            var actual = DictionaryUtils.DefaultIfEmpty(col, () => new Dictionary<int, int> { { 0, 0 } });
            Assert.Single(actual);
            Assert.Equal(1, actual[1]);
        }

        [Fact]
        public void Test_EmptyIfNull()
        {
            var col = new Dictionary<int, int>
            {
            };

            var actual = DictionaryUtils.EmptyIfNull(col);
            Assert.True(object.ReferenceEquals(col, actual));
        }

        [Fact]
        public void Test_EmptyIfNull_if_null()
        {
            Dictionary<int, int> col = null;

            var actual = DictionaryUtils.EmptyIfNull(col);
            Assert.Empty(actual);
        }


        [Fact]
        public void Test_IsEmpty()
        {
            var col = new Dictionary<int, int>
            {
            };

            var actual = DictionaryUtils.IsEmpty(col);
            Assert.True(actual);
        }

        [Fact]
        public void Test_IsEmpty_if_notEmpty()
        {
            var col = new Dictionary<int, int>
            {
                { 1, 1 }
            };

            var actual = DictionaryUtils.IsEmpty(col);
            Assert.False(actual);
        }

        [Fact]
        public void Test_IsEmpty_if_null()
        {
            Dictionary<int, int> col = null;

            var actual = DictionaryUtils.IsEmpty(col);
            Assert.True(actual);
        }

        [Fact]
        public void Test_IsNotEmpty()
        {
            var col = new Dictionary<int, int>
            {
                { 1, 1 }
            };

            var actual = DictionaryUtils.IsNotEmpty(col);
            Assert.True(actual);
        }

        [Fact]
        public void Test_IsNotEmpty_if_empty()
        {
            var col = new Dictionary<int, int>
            {
            };

            var actual = DictionaryUtils.IsNotEmpty(col);
            Assert.False(actual);
        }

        [Fact]
        public void Test_IsNotEmpty_if_null()
        {
            Dictionary<int, int> col = null;

            var actual = DictionaryUtils.IsNotEmpty(col);
            Assert.False(actual);
        }
    }
}
