using RockHouse.Collections.Slots;
using Xunit;

namespace Tests.Slots
{
    public class SlotTest
    {
        [Fact]
        public void Test_Create_1()
        {
            var actual = Slot.Create("1");
            Assert.Equal("1", actual.Item1);
        }

        [Fact]
        public void Test_Create_2()
        {
            var actual = Slot.Create("1", "2");
            Assert.Equal("1", actual.Item1);
            Assert.Equal("2", actual.Item2);
        }
    }
}
