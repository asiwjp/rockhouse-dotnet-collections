using System.Linq;
using Xunit;

namespace RockHouse.Collections.Tests
{
    public class TestBaseTest : TestBase
    {
        [Fact]
        public void Test_EqualsValue()
        {
            var o1 = new TestPoco
            {
                Primitive = 10,
                PrimitiveArray = new[] { 20, 30 },
                Str = "str1",
                Complex = new TestPoco
                {
                    Primitive = 100,
                }
            };
            var o2 = new TestPoco
            {
                Primitive = 10,
                PrimitiveArray = new[] { 20, 30 },
                Str = "str1",
                Complex = new TestPoco
                {
                    Primitive = 100,
                }
            };
            EqualsValue(o1, o2);
        }

        [Fact]
        public void Test_EqualsValue_int()
        {
            EqualsValue(1, 1);
        }

        [Fact]
        public void Test_EqualsValue_string()
        {
            EqualsValue("a", "a");
        }


        [Fact]
        public void Test_EqualsValue_null()
        {
            EqualsValue(null, null);
        }

        [Fact]
        public void Test_EqualsValue_DateTimeOffset()
        {
            var o1 = ToDateTimeOffset("2023-01-01");
            var o2 = ToDateTimeOffset("2023-01-01");
            EqualsValue(o1, o2);
        }

        [Fact]
        public void Test_EqualsValue_ref()
        {
            var o1 = new TestPoco();
            EqualsValue(o1, o1);
        }

        [Fact]
        public void Test_EqualsValue_enum()
        {
            EqualsValue(new string[] { "a" }, new string[] { "a" });
        }

        [Fact]
        public void Test_EqualsValue_if_type_missmatch()
        {
            var o1 = new TestPoco();

            try
            {
                EqualsValue(o1, "a", "obj");
            }
            catch (DiffException e)
            {
                Assert.Equal("obj", e.Path);
            }
        }

        [Fact]
        public void Test_EqualsProps()
        {
            var o1 = new TestPoco
            {
                Primitive = 10,
                PrimitiveArray = new[] { 20, 30 },
                Str = "str1",
                Complex = new TestPoco
                {
                    Primitive = 100,
                }
            };
            var o2 = new TestPoco
            {
                Primitive = 10,
                PrimitiveArray = new[] { 20, 30 },
                Str = "str1",
                Complex = new TestPoco
                {
                    Primitive = 100,
                }
            };
            EqualsProps(o1, o2);
        }

        [Fact]
        public void Test_EqualsProps_if_diff()
        {
            var o1 = new TestPoco
            {
                Str = "str1",
            };
            var o2 = new TestPoco
            {
                Str = "****************",
            };

            try
            {
                EqualsProps(o1, o2, "obj");
                Assert.Fail("Should be throw DiffException");
            }
            catch (DiffException e)
            {
                Assert.Equal("obj.Str", e.Path);
            }
        }

        [Fact]
        public void Test_EqualsEnumerable()
        {
            var enum1 = new int[] { 1, 2 };
            var enum2 = new int[] { 1, 2 };

            EqualsEnumerable(enum1, enum2);
        }

        [Fact]
        public void Test_EqualsEnumerable_if_element_not_match()
        {
            var enum1 = new int[] { 1, 2 };
            var enum2 = new int[] { 1, 1 };

            try
            {
                EqualsEnumerable(enum1, enum2, "array");
                Assert.Fail("Should be thorw DiffException.");
            }
            catch (DiffException e)
            {
                Assert.Equal($"array[1]", e.Path);
            }
        }

        [Theory]
        [InlineData("1", "")]
        [InlineData("", "1")]
        [InlineData("1,2", "1")]
        [InlineData("1", "1,2")]
        public void Test_EqualsEnumerable_if_length_not_match(string enum1src, string enum2src)
        {
            var enum1 = enum1src.Split(',').Where(x => x != "").ToList();
            var enum2 = enum2src.Split(',').Where(x => x != "").ToList();

            try
            {
                EqualsEnumerable(enum1, enum2, "array");
                Assert.Fail("Should be thorw DiffException.");
            }
            catch (DiffException e)
            {
                Assert.Equal($"array", e.Path);
            }
        }

        public class TestPoco
        {
            public int Primitive { get; set; }
            public string Str { get; set; }
            public int[] PrimitiveArray { get; set; }
            public TestPoco Complex { get; set; }
        }

    }
}
