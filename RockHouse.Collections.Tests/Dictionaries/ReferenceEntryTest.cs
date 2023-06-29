using RockHouse.Collections;
using RockHouse.Collections.Dictionaries;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Dictionaries
{
    public class ReferenceEntryTest : TestBase
    {
        [Fact]
        public void Test_ctor()
        {
            var entry = new ReferenceEntry<string, string>(ReferenceStrength.Weak, "a", ReferenceStrength.Weak, "b", null);

            Assert.True(entry.Key.Equals("a"));
            Assert.True(entry.Value.Equals("b"));
        }

        [Fact]
        public void Test_GetKeyValue()
        {
            var entry = new ReferenceEntry<string, string>(
                ReferenceStrength.Weak,
                "key",
                ReferenceStrength.Weak,
                "value",
                null
                );

            var actual = entry.GetKeyValue();
            Assert.NotNull(actual);
            Assert.Equal("key", actual.Value.Key);
            Assert.Equal("value", actual.Value.Value);
        }

        [Fact]
        public void Test_GetKeyValue_with_weakref_after_gc()
        {
            var entry = new ReferenceEntry<Tuple<string>, Tuple<string>>(
                ReferenceStrength.Weak,
                new Tuple<string>("key"),
                ReferenceStrength.Weak,
                new Tuple<string>("value"),
                null
                );
            Task.Run(() =>
            {
                entry.Key.Set(new Tuple<string>("xxx"));
                entry.Value.Set(new Tuple<string>("xxx"));
            }).Wait();
            ForceGC();

            var actual = entry.GetKeyValue();
            Assert.Null(actual);
        }
    }
}
