using RockHouse.Collections.Dictionaries;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace RockHouse.Collections.Tests.Dictionaries.Json.SystemTextJson
{
    public partial class DictionaryJsonConverterTest
    {
        private static void AssertContainsKey<K, V>(IOrderedDictionary<K, V> actual, K key)
        {
            Assert.True(actual.ContainsKey(key));
        }

        private static void AssertEqual<K, V>(IOrderedDictionary<K, V> expected, IOrderedDictionary<K, V> actual)
        {
            Assert.Equal(expected, actual);

            var expectedMap = (IOrderedDictionary<K, V>)expected;
            var actualMap = (IOrderedDictionary<K, V>)actual;
            Assert.Equal(expectedMap.FirstKey, actualMap.FirstKey);
            Assert.Equal(expectedMap.LastKey, actualMap.LastKey);
        }

        [Fact]
        public void Test___ctor_supportedKeyType()
        {
            var boolMap = JsonSerializer.Deserialize<LinkedHashMap<bool, int>>(@"{""true"":1}");
            Assert.Equal(1, boolMap[true]);

            var sbyteMap = JsonSerializer.Deserialize<LinkedHashMap<sbyte, int>>(@"{""2"":1}");
            Assert.Equal(1, sbyteMap[2]);

            var shortMap = JsonSerializer.Deserialize<LinkedHashMap<short, int>>(@"{""3"":1}");
            Assert.Equal(1, shortMap[3]);

            var intMap = JsonSerializer.Deserialize<LinkedHashMap<int, int>>(@"{""4"":1}");
            Assert.Equal(1, intMap[4]);

            var longMap = JsonSerializer.Deserialize<LinkedHashMap<long, int>>(@"{""5"":1}");
            Assert.Equal(1, longMap[5]);

            var floatMap = JsonSerializer.Deserialize<LinkedHashMap<float, int>>(@"{""6.1"":1}");
            Assert.Equal(1, floatMap[6.1f]);

            var doubleMap = JsonSerializer.Deserialize<LinkedHashMap<double, int>>(@"{""7.1"":1}");
            Assert.Equal(1, doubleMap[7.1d]);

            var byteMap = JsonSerializer.Deserialize<LinkedHashMap<sbyte, int>>(@"{""2"":1}");
            Assert.Equal(1, byteMap[2]);

            var ushortMap = JsonSerializer.Deserialize<LinkedHashMap<short, int>>(@"{""3"":1}");
            Assert.Equal(1, ushortMap[3]);

            var uintMap = JsonSerializer.Deserialize<LinkedHashMap<int, int>>(@"{""4"":1}");
            Assert.Equal(1, uintMap[4]);

            var ulongMap = JsonSerializer.Deserialize<LinkedHashMap<long, int>>(@"{""5"":1}");
            Assert.Equal(1, ulongMap[5]);

            var ufloatMap = JsonSerializer.Deserialize<LinkedHashMap<float, int>>(@"{""6.1"":1}");
            Assert.Equal(1, ufloatMap[6.1f]);

            var udoubleMap = JsonSerializer.Deserialize<LinkedHashMap<double, int>>(@"{""7.1"":1}");
            Assert.Equal(1, udoubleMap[7.1d]);

            var charMap = JsonSerializer.Deserialize<LinkedHashMap<char, int>>(@"{""8"":1}");
            Assert.Equal(1, charMap['8']);

            var stringMap = JsonSerializer.Deserialize<LinkedHashMap<string, int>>(@"{""string"":1}");
            Assert.Equal(1, stringMap["string"]);

            // TODO
            //var dateTimeMap = JsonSerializer.Deserialize<LinkedHashMap<DateTime, int>>(@"{""2019-07-26T16:59:57"":1}");
            //var dateTimeOffsetMap = JsonSerializer.Deserialize<LinkedHashMap<DateTimeOffset, int>>(@"{""2019-07-26T16:59:57+09:00"":1}");
            //var enumMap = JsonSerializer.Deserialize<LinkedHashMap<DayOfWeek, int>>(@"{""Monnay"":1}");
            //var guidMap = JsonSerializer.Deserialize<LinkedHashMap<Guid, int>>(@"{""3F2504E0-4F89-11D3-9A0C-0305E82C3301"":1}");
            //var uriMap = JsonSerializer.Deserialize<LinkedHashMap<Uri, int>>(@"{""https://localhost"":1}");
        }

        [Fact]
        public void Test___ctor_not_supportedKeyType()
        {
            Assert.Throws<NotSupportedException>(() => JsonSerializer.Deserialize<LinkedHashMap<string[], int>>(@"{}"));
        }

        [Fact]
        public void Test___ctor_not_supportedValueType()
        {
#if !NET5_0_OR_GREATER
            Assert.Throws<NotSupportedException>(() => JsonSerializer.Deserialize<LinkedHashMap<string, Action>>(@"{"""":1}"));
#endif
        }

        [Fact]
        public void Test_Read()
        {
            var expected = new LinkedHashMap<string, int>();
            expected.Add("b", 1);
            expected.Add("a", 2);

            var json = @"{""b"":1,""a"":2}";
            var actual = JsonSerializer.Deserialize<LinkedHashMap<string, int>>(json);

            AssertEqual(expected, actual);
        }

        [Fact]
        public void Test_Read_empty()
        {
            var json = @"";
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<LinkedHashMap<string, int>>(json));
        }

        [Fact]
        public void Test_Read_emptyObject()
        {
            var json = @"{}";
            var actual = JsonSerializer.Deserialize<LinkedHashMap<string, int>>(json);

            Assert.Empty(actual);
        }

        [Theory]
        [InlineData(@"[]")]
        [InlineData(@"}")]
        [InlineData(@"{")]
        [InlineData(@"{1}")]
        [InlineData(@"{[:}")]
        [InlineData(@"{""key""")]
        [InlineData(@"{""key"":")]
        [InlineData(@"{""key"":}")]
        [InlineData(@"{""key"":1")]
        [InlineData(@"{""key"":1,")]
        [InlineData(@"{""key"":,""key2"":2}")]
        public void Test_Read_malformed_jsons(string json)
        {
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<LinkedHashMap<string, int>>(json));
        }

        [Fact]
        public void Test_Read_value_is_null_under_primitiveType()
        {
            var json = @"{""key"":null}";
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<LinkedHashMap<string, int>>(json));
        }

        [Fact]
        public void Test_Read_value_is_null_under_referenceType()
        {
            var json = @"{""key"":null}";
            var actual = JsonSerializer.Deserialize<LinkedHashMap<string, string>>(json);
            Assert.Null(actual["key"]);
        }

        [Fact]
        public void Test_Write()
        {
            var col = new LinkedHashMap<string, int>();
            col.Add("b", 1);
            col.Add("a", 2);

            var actual = JsonSerializer.Serialize(col);
            Assert.Equal(@"{""b"":1,""a"":2}", actual);
        }

        [Fact]
        public void Test_Write_empty()
        {
            var col = new LinkedHashMap<string, string>();

            var actual = JsonSerializer.Serialize(col);
            Assert.Equal(@"{}", actual);
        }

        [Fact]
        public void Test_Write_value_is_null()
        {
            var col = new LinkedHashMap<string, string>();
            col.Add("b", "1");
            col.Add("a", null);

            var actual = JsonSerializer.Serialize(col);
            Assert.Equal(@"{""b"":""1"",""a"":null}", actual);
        }

        [Fact]
        public void Test_Write_value_is_null_with_ignoreNull()
        {
            var col = new LinkedHashMap<string, string>();
            col.Add("b", "1");
            col.Add("a", null);

            var options = new JsonSerializerOptions();
            options.IgnoreNullValues = true;
            var actual = JsonSerializer.Serialize(col, options);
            Assert.Equal(@"{""b"":""1"",""a"":null}", actual);  // null was not removed.

            // Dictionary compatibility
            var std = new Dictionary<string, string>(col);
            var stdResult = JsonSerializer.Serialize(std, options);
            Assert.Equal(stdResult, actual);
        }
    }
}
