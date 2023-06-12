using RockHouse.Collections.ReferenceHolders;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RockHouse.Collections.Tests.ReferenceHolders
{
    public class HardReferenceHolderTest : TestBase
    {
        [Fact]
        public void Test_Equals()
        {
            var holder = new HardReferenceHolder("a");

            var actual1 = holder.Equals("a");
            Assert.True(actual1);

            var actual2 = holder.Equals("b");
            Assert.False(actual2);
        }

        [Fact]
        public void Test_GetHashCode()
        {
            var holder = new HardReferenceHolder("a");

            var actual1 = holder.GetHashCode();
            Assert.Equal("a".GetHashCode(), actual1);
        }

        [Fact]
        public void Test_GetHashCode_after_replace()
        {
            var holder = new HardReferenceHolder("a");
            holder.Set("b");

            var actual1 = holder.GetHashCode();
            Assert.Equal("b".GetHashCode(), actual1);
        }

        [Fact]
        public void Test_Set()
        {
            var holder = new HardReferenceHolder("old");
            holder.Set("new");

            var actualResult = holder.TryGet(out var actualObj);
            Assert.True(actualResult);
            Assert.Equal("new", actualObj);
        }

        [Fact]
        public void Test_TryGet()
        {
            var holder = new HardReferenceHolder("a");
            var actualResult = holder.TryGet(out var actualObj);
            Assert.True(actualResult);
            Assert.Equal("a", actualObj);
        }

        [Fact]
        public void Test_TryGet_with_gc()
        {
            var holder = new HardReferenceHolder(Tuple.Create("old"));
            Task.Run(() =>
            {
                holder.Set(Tuple.Create("new"));
            }).Wait();
            ForceGC();

            var actualResult = holder.TryGet(out var actualObj);
            Assert.True(actualResult);
            Assert.Equal(Tuple.Create("new"), actualObj);
        }
    }
}
