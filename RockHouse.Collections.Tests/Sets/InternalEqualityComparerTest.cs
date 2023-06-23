using RockHouse.Collections.Sets;
using System;
using System.Collections.Generic;
using Xunit;

namespace RockHouse.Collections.Tests.Sets
{
    public class InternalEqualityComparerTest
    {
        [Fact]
        public void Test_ctor()
        {
            var orgComparer = new IgnoreCaseStringComparer();
            var comparer = new InternalEqualityComparer<string>(orgComparer);
            Assert.True(object.ReferenceEquals(comparer.EqualityComparer, orgComparer));
        }

        [Fact]
        public void Test_ctor__with_null()
        {
            var comparer = new InternalEqualityComparer<string>(null);
            Assert.True(object.ReferenceEquals(EqualityComparer<string>.Default, comparer.EqualityComparer));
        }

        [Fact]
        public void Test_Equals()
        {
            var comparer = new InternalEqualityComparer<string>(new IgnoreCaseStringComparer());
            Assert.True(comparer.Equals(ValueTuple.Create("a"), ValueTuple.Create("A")));
            Assert.True(comparer.Equals(ValueTuple.Create("A"), ValueTuple.Create("A")));
        }

        [Fact]
        public void Test_GetHashCode()
        {
            var comparer = new InternalEqualityComparer<string>(new IgnoreCaseStringComparer());
            Assert.Equal("A".GetHashCode(), comparer.GetHashCode(ValueTuple.Create("a")));
        }
    }
}
