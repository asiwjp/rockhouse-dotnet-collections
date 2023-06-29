using RockHouse.Collections.Sets;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace Tests.Sets.Json.SystemTextJson
{
    public partial class SetJsonConverterTest
    {
        private static void AssertContains<T>(IOrderedSet<T> actual, T value)
        {
            Assert.True(actual.Contains(value));
        }

        private static void AssertEqual<T>(IOrderedSet<T> expected, IOrderedSet<T> actual)
        {
            Assert.Equal(expected, actual);

            var expectedMap = (IOrderedSet<T>)expected;
            var actualMap = (IOrderedSet<T>)actual;
            Assert.Equal(expectedMap.First, actualMap.First);
            Assert.Equal(expectedMap.Last, actualMap.Last);
        }

        [Fact]
        public void Test___ctor_not_supportedElementType()
        {
#if !NET5_0_OR_GREATER
            Assert.Throws<NotSupportedException>(() => JsonSerializer.Deserialize<LinkedHashSet<Action>>(@"{"""":1}"));
#endif
        }

        [Fact]
        public void Test_Read()
        {
            var expected = new LinkedHashSet<string>();
            expected.Add("b");
            expected.Add("a");

            var json = @"[""b"",""a""]";
            var actual = JsonSerializer.Deserialize<LinkedHashSet<string>>(json);

            AssertEqual(expected, actual);
        }

        [Fact]
        public void Test_Read_empty()
        {
            var json = @"";
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<LinkedHashSet<string>>(json));
        }

        [Fact]
        public void Test_Read_emptyArray()
        {
            var json = @"[]";
            var actual = JsonSerializer.Deserialize<LinkedHashSet<string>>(json);
            Assert.Empty(actual);
        }

        [Theory]
        [InlineData(@"{}")]
        [InlineData(@"}")]
        [InlineData(@"{")]
        [InlineData(@"[")]
        [InlineData(@"[,")]
        [InlineData(@"[}")]
        public void Test_Read_malformed_jsons(string json)
        {
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<LinkedHashSet<string>>(json));
        }

        [Fact]
        public void Test_Read_value_is_null_under_primitiveType()
        {
            var json = @"[null]";
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<LinkedHashSet<int>>(json));
        }

        [Fact]
        public void Test_Read_value_is_null_under_referenceType()
        {
            var json = @"[null]";
            var actual = JsonSerializer.Deserialize<LinkedHashSet<string>>(json);
            Assert.Null(actual.First);
        }

        [Fact]
        public void Test_Write()
        {
            var col = new LinkedHashSet<string>();
            col.Add("b");
            col.Add("a");

            var actual = JsonSerializer.Serialize(col);
            Assert.Equal(@"[""b"",""a""]", actual);
        }

        [Fact]
        public void Test_Write_empty()
        {
            var col = new LinkedHashSet<string>();

            var actual = JsonSerializer.Serialize(col);
            Assert.Equal(@"[]", actual);
        }

        [Fact]
        public void Test_Write_value_is_null()
        {
            var col = new LinkedHashSet<string>();
            col.Add("b");
            col.Add(null);

            var actual = JsonSerializer.Serialize(col);
            Assert.Equal(@"[""b"",null]", actual);
        }

        [Fact]
        public void Test_Write_value_is_null_with_ignoreNull()
        {
            var col = new LinkedHashSet<string>();
            col.Add("b");
            col.Add(null);

            var options = new JsonSerializerOptions();
            options.IgnoreNullValues = true;
            var actual = JsonSerializer.Serialize(col, options);
            Assert.Equal(@"[""b"",null]", actual);  // null was not removed.

            // Dictionary compatibility
            var std = new HashSet<string>(col);
            var stdResult = JsonSerializer.Serialize(std, options);
            Assert.Equal(stdResult, actual);
        }
    }
}
