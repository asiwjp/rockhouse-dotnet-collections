using RockHouse.Collections.ReferenceHolders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.ReferenceHolders
{
    public class HardReferenceHolderTest : TestBase
    {
        [Fact]
        public void Test__with_comparer__Equals()
        {
            var holder = new HardReferenceHolder("a", new IgnoreCaseStringComparer());

            var actual1 = holder.Equals("a");
            Assert.True(actual1);

            var actual2 = holder.Equals("A");
            Assert.True(actual2);

            var actual3 = holder.Equals("b");
            Assert.False(actual3);
        }

        [Fact]
        public void Test__with_comparer__GetHashCode()
        {
            var holder = new HardReferenceHolder("a", new IgnoreCaseStringComparer());

            var actual1 = holder.GetHashCode();
            Assert.Equal("A".GetHashCode(), actual1);

            holder.Set("A");
            var actual2 = holder.GetHashCode();
            Assert.Equal(actual1, actual2);
        }


        [Fact]
        public void Test_Equals()
        {
            var holder = new HardReferenceHolder("a", null);

            var actual1 = holder.Equals("a");
            Assert.True(actual1);

            var actual2 = holder.Equals("b");
            Assert.False(actual2);
        }

        [Fact]
        public void Test_GetHashCode()
        {
            var holder = new HardReferenceHolder("a", null);

            var actual1 = holder.GetHashCode();
            Assert.Equal("a".GetHashCode(), actual1);
        }

        [Fact]
        public void Test_GetHashCode_after_replace()
        {
            var holder = new HardReferenceHolder("a", null);
            holder.Set("b");

            var actual1 = holder.GetHashCode();
            Assert.Equal("b".GetHashCode(), actual1);
        }

        [Fact]
        public void Test_Set()
        {
            var holder = new HardReferenceHolder("old", null);
            holder.Set("new");

            var actualResult = holder.TryGet(out var actualObj);
            Assert.True(actualResult);
            Assert.Equal("new", actualObj);
        }

        [Fact]
        public void Test_TryGet()
        {
            var holder = new HardReferenceHolder("a", null);
            var actualResult = holder.TryGet(out var actualObj);
            Assert.True(actualResult);
            Assert.Equal("a", actualObj);
        }

        [Fact]
        public void Test_TryGet_with_gc()
        {
            var holder = new HardReferenceHolder(Tuple.Create("old"), null);
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
