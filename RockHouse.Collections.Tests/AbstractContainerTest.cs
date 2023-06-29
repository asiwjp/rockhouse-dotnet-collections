using RockHouse.Collections;
using System;
using Xunit;

namespace Tests
{
    public class AbstractContainerTest
    {
        [Fact]
        public void Test_IsEmpty()
        {
            var col = new Stub();
            col.InvokeCheckEmpty();
            // if does not throw, ok

            col._isEmpty = true;
            Assert.Throws<InvalidOperationException>(() => col.InvokeCheckEmpty());
        }

        public class Stub : AbstractCollection
        {
            public bool _isEmpty;

            public override bool IsEmpty { get => _isEmpty; }

            public void InvokeCheckEmpty() => this.CheckEmpty();
        }
    }
}
