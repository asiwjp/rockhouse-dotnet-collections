using RockHouse.Collections.ReferenceHolders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RockHouse.Collections.Tests.ReferenceHolders
{
    public class WeakReferenceHolderTest : TestBase
    {
        [Fact]
        public void Test_Equals()
        {
            var holder = new WeakReferenceHolder(Tuple.Create("a"));

            var actual1 = holder.Equals(Tuple.Create("a"));
            Assert.True(actual1);

            var actual2 = holder.Equals(Tuple.Create("b"));
            Assert.False(actual2);
        }

        [Fact]
        public void Test_Equals_with_gc()
        {
            var holder = new WeakReferenceHolder(Tuple.Create("a"));
            Task.Run(() =>
            {
                holder.Set(Tuple.Create("xxx"));
            }).Wait();
            ForceGC();

            var actual1 = holder.Equals(Tuple.Create("a"));
            Assert.False(actual1);
        }

        [Fact]
        public void Test_GetHashCode()
        {
            var holder = new WeakReferenceHolder("a");

            var actual1 = holder.GetHashCode();
            Assert.Equal("a".GetHashCode(), actual1);
        }

        [Fact]
        public void Test_GetHashCode_after_replace()
        {
            var holder = new WeakReferenceHolder("a");
            holder.Set("b");

            var actual1 = holder.GetHashCode();
            Assert.Equal("b".GetHashCode(), actual1);
        }

        [Fact]
        public void Test_Set()
        {
            var holder = new WeakReferenceHolder("old");
            holder.Set("new");

            var actualResult = holder.TryGet(out var actualObj);
            Assert.True(actualResult);
            Assert.Equal("new", actualObj);
        }

        [Fact]
        public void Test_TryGet()
        {
            var holder = new WeakReferenceHolder("a");
            var actualResult = holder.TryGet(out var actualObj);
            Assert.True(actualResult);
            Assert.Equal("a", actualObj);
        }

        [Fact]
        public void Test_TryGet_with_gc()
        {
            var holder = new WeakReferenceHolder(Tuple.Create("old"));
            Task.Run(() =>
            {
                holder.Set(Tuple.Create("xxx"));
            }).Wait();
            ForceGC();

            var actualResult = holder.TryGet(out var actualObj);
            Assert.False(actualResult);
            Assert.Null(actualObj);
        }
    }
}
