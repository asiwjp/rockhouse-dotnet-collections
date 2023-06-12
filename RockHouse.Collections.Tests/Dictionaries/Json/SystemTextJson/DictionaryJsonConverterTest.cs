using RockHouse.Collections.Dictionaries;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace RockHouse.Collections.Tests.Dictionaries.Json.SystemTextJson
{
    public partial class DictionaryJsonConverterTest : TestBase
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
        public void Test___types()
        {
            string json = @"{""True"":false}";
            var boolMap = JsonSerializer.Deserialize<LinkedHashMap<bool, bool>>(json);
            var actualJson = JsonSerializer.Serialize(boolMap);
            Assert.False(boolMap[true]);
            Assert.Equal(json, actualJson);

            json = @"{""2"":1}";
            var sbyteMap = JsonSerializer.Deserialize<LinkedHashMap<sbyte, int>>(json);
            actualJson = JsonSerializer.Serialize(sbyteMap);
            Assert.Equal(1, sbyteMap[2]);
            Assert.Equal(json, actualJson);

            json = @"{""3"":1}";
            var shortMap = JsonSerializer.Deserialize<LinkedHashMap<short, int>>(json);
            actualJson = JsonSerializer.Serialize(shortMap);
            Assert.Equal(1, shortMap[3]);
            Assert.Equal(json, actualJson);

            json = @"{""4"":1}";
            var intMap = JsonSerializer.Deserialize<LinkedHashMap<int, int>>(json);
            actualJson = JsonSerializer.Serialize(intMap);
            Assert.Equal(1, intMap[4]);
            Assert.Equal(json, actualJson);

            json = @"{""5"":1}";
            var longMap = JsonSerializer.Deserialize<LinkedHashMap<long, int>>(json);
            actualJson = JsonSerializer.Serialize(longMap);
            Assert.Equal(1, longMap[5]);
            Assert.Equal(json, actualJson);

            json = @"{""6.1"":1}";
            var floatMap = JsonSerializer.Deserialize<LinkedHashMap<float, int>>(json);
            actualJson = JsonSerializer.Serialize(floatMap);
            Assert.Equal(1, floatMap[6.1f]);
            Assert.Equal(json, actualJson);

            json = @"{""7.1"":1}";
            var doubleMap = JsonSerializer.Deserialize<LinkedHashMap<double, int>>(json);
            actualJson = JsonSerializer.Serialize(doubleMap);
            Assert.Equal(1, doubleMap[7.1d]);
            Assert.Equal(json, actualJson);

            json = @"{""2"":1}";
            var byteMap = JsonSerializer.Deserialize<LinkedHashMap<byte, int>>(json);
            actualJson = JsonSerializer.Serialize(byteMap);
            Assert.Equal(1, byteMap[2]);
            Assert.Equal(json, actualJson);

            json = @"{""3"":1}";
            var ushortMap = JsonSerializer.Deserialize<LinkedHashMap<ushort, int>>(json);
            actualJson = JsonSerializer.Serialize(ushortMap);
            Assert.Equal(1, ushortMap[3]);
            Assert.Equal(json, actualJson);

            json = @"{""4"":1}";
            var uintMap = JsonSerializer.Deserialize<LinkedHashMap<uint, int>>(json);
            actualJson = JsonSerializer.Serialize(uintMap);
            Assert.Equal(1, uintMap[4]);
            Assert.Equal(json, actualJson);

            json = @"{""5"":1}";
            var ulongMap = JsonSerializer.Deserialize<LinkedHashMap<ulong, int>>(json);
            actualJson = JsonSerializer.Serialize(ulongMap);
            Assert.Equal(1, ulongMap[5]);
            Assert.Equal(json, actualJson);

            json = @"{""8"":1}";
            var charMap = JsonSerializer.Deserialize<LinkedHashMap<char, int>>(json);
            actualJson = JsonSerializer.Serialize(charMap);
            Assert.Equal(1, charMap['8']);
            Assert.Equal(json, actualJson);

            json = @"{""key"":""value""}";
            var stringMap = JsonSerializer.Deserialize<LinkedHashMap<string, string>>(json);
            actualJson = JsonSerializer.Serialize(stringMap);
            Assert.Equal("value", stringMap["key"]);
            Assert.Equal(json, actualJson);

            // TODO
            //var dateTimeMap = JsonSerializer.Deserialize<LinkedHashMap<DateTime, int>>(@"{""2019-07-26T16:59:57"":1}");
            //var enumMap = JsonSerializer.Deserialize<LinkedHashMap<DayOfWeek, int>>(@"{""Monnay"":1}");

            json = @"{""2019-07-26T16:59:57.9876543\u002B09:00"":""2020-07-26T16:59:57.9876543+09:00""}";
            var dtoMap = JsonSerializer.Deserialize<LinkedHashMap<DateTimeOffset, DateTimeOffset>>(json);
            actualJson = JsonSerializer.Serialize(dtoMap);
            Assert.Equal(DateTimeOffset.Parse("2020-07-26T16:59:57.9876543+09:00"), dtoMap[DateTimeOffset.Parse("2019-07-26T16:59:57.9876543+09:00")]);
            Assert.Equal(json, actualJson);

            json = @"{""3f2504e0-4f89-11d3-9a0c-0305e82c3301"":""6076179e-8803-4162-a7c8-849a0fd526bb""}";
            var guidMap = JsonSerializer.Deserialize<LinkedHashMap<Guid, Guid>>(json);
            actualJson = JsonSerializer.Serialize(guidMap);
            Assert.Equal(Guid.Parse("6076179e-8803-4162-a7c8-849a0fd526bb"), guidMap[Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")]);
            Assert.Equal(json, actualJson);

            json = @"{""https://localhost/"":""https://localhost:8181/path?a=b""}";
            var uriMap = JsonSerializer.Deserialize<LinkedHashMap<Uri, Uri>>(json);
            actualJson = JsonSerializer.Serialize(uriMap);
            Assert.Equal(new Uri("https://localhost:8181/path?a=b"), uriMap[new Uri("https://localhost/")]);
            Assert.Equal(json, actualJson);
        }


        [Fact]
        public void Test___types_not_supportedKeyType()
        {
            Assert.Throws<NotSupportedException>(() => JsonSerializer.Deserialize<LinkedHashMap<string[], int>>(@"{}"));
        }

        [Fact]
        public void Test___types_not_supportedValueType()
        {
#if !NET5_0_OR_GREATER
            Assert.Throws<NotSupportedException>(() => JsonSerializer.Deserialize<LinkedHashMap<string, Action>>(@"{"""":1}"));
#endif
        }

        [Fact]
        public void Test__keyType_guid_dateTimeOffset_format()
        {
            var json = @"{""???"":1}";
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<LinkedHashMap<DateTimeOffset, int>>(json));
        }

        [Fact]
        public void Test__keyType_guid_invalid_format()
        {
            var json = @"{""???"":1}";
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<LinkedHashMap<Guid, int>>(json));
        }

        [Fact]
        public void Test__keyType_number_invalid_format()
        {
            var json = @"{""???"":1}";
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<LinkedHashMap<int, int>>(json));
        }

        [Fact]
        public void Test__keyType_number_overflow()
        {
            var json = @"{""-9999999"":1}";
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<LinkedHashMap<byte, int>>(json));
        }

        [Fact]
        public void Test__keyType_url_invalid_format()
        {
            var json = @"{""???"":1}";
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<LinkedHashMap<Uri, int>>(json));
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
        public void Test_Read_null()
        {
            var json = @"null";
            var actual = JsonSerializer.Deserialize<LinkedHashMap<string, int>>(json);
            Assert.Null(actual);
            var expectedStd = JsonSerializer.Deserialize<Dictionary<string, int>>(json);
            Assert.Equal(expectedStd, actual);
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
        public void Test_Read_value_is_complexObject()
        {
            var json = @"{""key"":{""Int"":10,""IntArray"":[20],""IntList"":[30],""Str"":""str10"",""StrArray"":[""str11""],""DateTimeOffset"":""2023-01-01T00:00:00+09:00"",""Complex"":{""Int"":100,""IntArray"":null,""IntList"":null,""Str"":null,""StrArray"":null,""DateTimeOffset"":""0001-01-01T00:00:00+00:00"",""Complex"":null}}}";
            var actuals = JsonSerializer.Deserialize<LinkedHashMap<string, ComplexType>>(json);

            var expected = new LinkedHashMap<string, ComplexType>
            {
                {
                    "key",
                    new ComplexType
                    {
                        Int = 10,
                        IntArray = new int[] { 20 },
                        IntList = new List<int> { 30 },
                        Str = "str10",
                        StrArray = new string[] { "str11" },
                        Complex = new ComplexType { Int = 100 },
                        DateTimeOffset = ToDateTimeOffset("2023-01-01T00:00:00+09:00"),
                    }
                }
            };

            Assert.Single(actuals);
            AssertEquals(expected["key"], actuals["key"]);
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
        public void Test_Write_null()
        {
            LinkedHashMap<string, string> col = null;

            var actual = JsonSerializer.Serialize(col);
            Assert.Equal(@"null", actual);
        }

        [Fact]
        public void Test_Write_empty()
        {
            var col = new LinkedHashMap<string, string>();

            var actual = JsonSerializer.Serialize(col);
            Assert.Equal(@"{}", actual);
        }

        [Fact]
        public void Test_Write_value_is_complexObject()
        {
            var col = new LinkedHashMap<string, ComplexType>
            {
                {
                    "key",
                    new ComplexType
                    {
                        Int = 10,
                        IntArray = new int[] { 20 },
                        IntList = new List<int> { 30 },
                        Str = "str10",
                        StrArray = new string[] { "str11" },
                        Complex = new ComplexType { Int = 100 },
                        DateTimeOffset = ToDateTimeOffset("2023-01-01T00:00:00+09:00"),
                    }
                }
            };

            var expected = @"{""key"":{""Int"":10,""IntArray"":[20],""IntList"":[30],""Str"":""str10"",""StrArray"":[""str11""],""DateTimeOffset"":""2023-01-01T00:00:00+09:00"",""Complex"":{""Int"":100,""IntArray"":null,""IntList"":null,""Str"":null,""StrArray"":null,""DateTimeOffset"":""0001-01-01T00:00:00+00:00"",""Complex"":null}}}";
            var actual = JsonSerializer.Serialize(col);
            Assert.Equal(expected, actual);

            var dic = new Dictionary<string, ComplexType>(col);
            var stdResult = JsonSerializer.Serialize(dic);
            Assert.Equal(stdResult, actual);
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
