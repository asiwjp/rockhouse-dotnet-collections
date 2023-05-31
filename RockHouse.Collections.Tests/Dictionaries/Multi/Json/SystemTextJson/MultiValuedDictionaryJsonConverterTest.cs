using RockHouse.Collections.Dictionaries.Multi;
using RockHouse.Collections.Tests.Dictionaries.Json;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace RockHouse.Collections.Tests.Dictionaries.Multi.Json.SystemTextJson
{
    public partial class MultiValuedDictionaryJsonConverterTest : TestBase
    {
        private static void AssertEqual<K, V, C>(IMultiValuedDictionary<K, V, C> expected, IMultiValuedDictionary<K, V, C> actual)
            where C : ICollection<V>
        {
            Assert.Equal(expected, actual);

            var expectedMap = (IMultiValuedDictionary<K, V, C>)expected;
            var actualMap = (IMultiValuedDictionary<K, V, C>)actual;

            Assert.Equal(expectedMap.Keys, actualMap.Keys);
            foreach (var key in expectedMap.Keys)
            {
                Assert.Equal(expectedMap[key], actualMap[key]);
            }
        }

        [Fact]
        public void Test___ctor_supportedKeyType()
        {
            var boolMap = JsonSerializer.Deserialize<ListValuedMap<bool, int>>(@"{""true"":[1]}");
            Assert.Equal(1, boolMap[true][0]);

            var sbyteMap = JsonSerializer.Deserialize<ListValuedMap<sbyte, int>>(@"{""2"":[1]}");
            Assert.Equal(1, sbyteMap[2][0]);

            var shortMap = JsonSerializer.Deserialize<ListValuedMap<short, int>>(@"{""3"":[1]}");
            Assert.Equal(1, shortMap[3][0]);

            var intMap = JsonSerializer.Deserialize<ListValuedMap<int, int>>(@"{""4"":[1]}");
            Assert.Equal(1, intMap[4][0]);

            var longMap = JsonSerializer.Deserialize<ListValuedMap<long, int>>(@"{""5"":[1]}");
            Assert.Equal(1, longMap[5][0]);

            var floatMap = JsonSerializer.Deserialize<ListValuedMap<float, int>>(@"{""6.1"":[1]}");
            Assert.Equal(1, floatMap[6.1f][0]);

            var doubleMap = JsonSerializer.Deserialize<ListValuedMap<double, int>>(@"{""7.1"":[1]}");
            Assert.Equal(1, doubleMap[7.1d][0]);

            var byteMap = JsonSerializer.Deserialize<ListValuedMap<sbyte, int>>(@"{""2"":[1]}");
            Assert.Equal(1, byteMap[2][0]);

            var ushortMap = JsonSerializer.Deserialize<ListValuedMap<short, int>>(@"{""3"":[1]}");
            Assert.Equal(1, ushortMap[3][0]);

            var uintMap = JsonSerializer.Deserialize<ListValuedMap<int, int>>(@"{""4"":[1]}");
            Assert.Equal(1, uintMap[4][0]);

            var ulongMap = JsonSerializer.Deserialize<ListValuedMap<long, int>>(@"{""5"":[1]}");
            Assert.Equal(1, ulongMap[5][0]);

            var ufloatMap = JsonSerializer.Deserialize<ListValuedMap<float, int>>(@"{""6.1"":[1]}");
            Assert.Equal(1, ufloatMap[6.1f][0]);

            var udoubleMap = JsonSerializer.Deserialize<ListValuedMap<double, int>>(@"{""7.1"":[1]}");
            Assert.Equal(1, udoubleMap[7.1d][0]);

            var charMap = JsonSerializer.Deserialize<ListValuedMap<char, int>>(@"{""8"":[1]}");
            Assert.Equal(1, charMap['8'][0]);

            var stringMap = JsonSerializer.Deserialize<ListValuedMap<string, int>>(@"{""string"":[1]}");
            Assert.Equal(1, stringMap["string"][0]);

            // TODO
            //var dateTimeMap = JsonSerializer.Deserialize<ListValuedMap<DateTime, int>>(@"{""2019-07-26T16:59:57"":1}");
            //var dateTimeOffsetMap = JsonSerializer.Deserialize<ListValuedMap<DateTimeOffset, int>>(@"{""2019-07-26T16:59:57+09:00"":1}");
            //var enumMap = JsonSerializer.Deserialize<ListValuedMap<DayOfWeek, int>>(@"{""Monnay"":1}");
            //var guidMap = JsonSerializer.Deserialize<ListValuedMap<Guid, int>>(@"{""3F2504E0-4F89-11D3-9A0C-0305E82C3301"":1}");
            //var uriMap = JsonSerializer.Deserialize<ListValuedMap<Uri, int>>(@"{""https://localhost"":1}");
        }

        [Fact]
        public void Test___ctor_not_supportedKeyType()
        {
            Assert.Throws<NotSupportedException>(() => JsonSerializer.Deserialize<ListValuedMap<string[], int>>(@"{}"));
        }

        [Fact]
        public void Test___ctor_not_supportedValueType()
        {
#if !NET5_0_OR_GREATER
            Assert.Throws<NotSupportedException>(() => JsonSerializer.Deserialize<ListValuedMap<string, Action>>(@"{"""":1}"));
#endif
        }

        [Fact]
        public void Test_Read()
        {
            var expected = new ListValuedMap<string, int>();
            expected.Add("b", 1);
            expected.Add("b", 2);
            expected.Add("a", 3);

            var json = @"{""b"":[1,2],""a"":[3]}";
            var actual = JsonSerializer.Deserialize<ListValuedMap<string, int>>(json);

            AssertEqual(expected, actual);
        }

        [Fact]
        public void Test_Read_null()
        {
            var json = @"null";
            var actual = JsonSerializer.Deserialize<ListValuedMap<string, int>>(json);
            Assert.Null(actual);
            var expectedStd = JsonSerializer.Deserialize<Dictionary<string, int>>(json);
            Assert.Equal(expectedStd, actual);
        }
        [Fact]
        public void Test_Read_empty()
        {
            var json = @"";
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ListValuedMap<string, int>>(json));
        }
        [Fact]
        public void Test_Read_emptyObject()
        {
            var json = @"{}";
            var actual = JsonSerializer.Deserialize<ListValuedMap<string, int>>(json);
            Assert.Empty(actual);
        }

        [Theory]
        [InlineData(@"[]")]
        [InlineData(@"}")]
        [InlineData(@"{")]
        [InlineData(@"{]")]
        [InlineData(@"{1}")]
        [InlineData(@"{[:}")]
        [InlineData(@"{null:[1]}")]
        [InlineData(@"{""key""")]
        [InlineData(@"{""key"":")]
        [InlineData(@"{""key"":}")]
        [InlineData(@"{""key"":""not-array""}")]
        [InlineData(@"{""key"":1")]
        [InlineData(@"{""key"":1,")]
        [InlineData(@"{""key"":[")]
        [InlineData(@"{""key"":[1]")]
        [InlineData(@"{""key"":[1],")]
        [InlineData(@"{""key"":,""key2"":2}")]
        public void Test_Read_malformed_jsons(string json)
        {
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ListValuedMap<string, int>>(json));
        }

        [Fact]
        public void Test_Read_value_is_complexObject()
        {
            var json = @"{""key"":[{""Int"":10,""IntArray"":[20],""IntList"":[30],""Str"":""str10"",""StrArray"":[""str11""],""DateTimeOffset"":""2023-01-01T00:00:00+09:00"",""Complex"":{""Int"":100,""IntArray"":null,""IntList"":null,""Str"":null,""StrArray"":null,""DateTimeOffset"":""0001-01-01T00:00:00+00:00"",""Complex"":null}}]}";
            var actuals = JsonSerializer.Deserialize<ListValuedMap<string, ComplexType>>(json);

            var expected = new ListValuedMap<string, ComplexType>
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
        public void Test_Read_value_is_null()
        {
            var json = @"{""key"":null}";
            var col = JsonSerializer.Deserialize<ListValuedMap<string, int>>(json);
            Assert.Empty(col);
        }

        [Fact]
        public void Test_Read_value_is_emptyArray()
        {
            var json = @"{""key"":[]}";
            var col = JsonSerializer.Deserialize<ListValuedMap<string, int>>(json);
            Assert.Empty(col);
        }

        [Fact]
        public void Test_Write()
        {
            var col = new ListValuedMap<string, int>();
            col.Add("b", 1);
            col.Add("b", 2);
            col.Add("a", 3);

            var actual = JsonSerializer.Serialize(col);
            Assert.Equal(@"{""b"":[1,2],""a"":[3]}", actual);
        }

        [Fact]
        public void Test_Write_null()
        {
            ListValuedMap<string, string> col = null;

            var actual = JsonSerializer.Serialize(col);
            Assert.Equal(@"null", actual);
        }

        [Fact]
        public void Test_Write_empty()
        {
            var col = new ListValuedMap<string, string>();

            var actual = JsonSerializer.Serialize(col);
            Assert.Equal(@"{}", actual);
        }

        [Fact]
        public void Test_Write_value_is_complexObject()
        {
            var col = new ListValuedMap<string, ComplexType>
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

            var expected = @"{""key"":[{""Int"":10,""IntArray"":[20],""IntList"":[30],""Str"":""str10"",""StrArray"":[""str11""],""DateTimeOffset"":""2023-01-01T00:00:00+09:00"",""Complex"":{""Int"":100,""IntArray"":null,""IntList"":null,""Str"":null,""StrArray"":null,""DateTimeOffset"":""0001-01-01T00:00:00+00:00"",""Complex"":null}}]}";
            var actual = JsonSerializer.Serialize(col);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_Write_value_is_null()
        {
            var col = new ListValuedMap<string, string>();
            col.Add("b", "1");
            col.Add("a", null);

            var actual = JsonSerializer.Serialize(col);
            Assert.Equal(@"{""b"":[""1""],""a"":[null]}", actual);
        }

        [Fact]
        public void Test_Write_value_is_null_with_ignoreNull()
        {
            var col = new ListValuedMap<string, string>();
            col.Add("b", "1");
            col.Add("a", null);

            var options = new JsonSerializerOptions();
            options.IgnoreNullValues = true;
            var actual = JsonSerializer.Serialize(col, options);
            Assert.Equal(@"{""b"":[""1""],""a"":[null]}", actual);  // null was not removed.
        }
    }
}
