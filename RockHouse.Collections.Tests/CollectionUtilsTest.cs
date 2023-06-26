using System.Collections.Generic;
using Xunit;

namespace RockHouse.Collections.Tests
{
    public class CollectionUtilsTest
    {
        [Fact]
        public void Test_Count()
        {
            var col = new List<int>
            {
                1
            };

            var actual = CollectionUtils.Count(col);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void Test_Count_if_null()
        {
            List<int> col = null;
            var actual = CollectionUtils.Count(col);
            Assert.Equal(0, actual);
        }

        [Fact]
        public void Test_IsEmpty()
        {
            var col = new List<int>
            {
            };

            var actual = CollectionUtils.IsEmpty(col);
            Assert.True(actual);
        }

        [Fact]
        public void Test_IsEmpty_if_notEmpty()
        {
            var col = new List<int>
            {
                1
            };

            var actual = CollectionUtils.IsEmpty(col);
            Assert.False(actual);
        }

        [Fact]
        public void Test_IsNotEmpty()
        {
            var col = new List<int>
            {
                1
            };

            var actual = CollectionUtils.IsNotEmpty(col);
            Assert.True(actual);
        }

        [Fact]
        public void Test_IsNotEmpty_if_empty()
        {
            var col = new List<int>
            {
            };

            var actual = CollectionUtils.IsNotEmpty(col);
            Assert.False(actual);
        }
    }
}
