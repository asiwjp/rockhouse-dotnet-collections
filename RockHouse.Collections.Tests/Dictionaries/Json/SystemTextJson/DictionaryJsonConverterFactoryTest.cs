using RockHouse.Collections.Dictionaries;
using RockHouse.Collections.Dictionaries.Json.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace RockHouse.Collections.Tests.Dictionaries.Json.SystemTextJson
{
    public class DictionaryJsonConverterFactoryTest
    {
        [Theory]
        [InlineData(false, typeof(string))]
        [InlineData(false, typeof(Dictionary<string, string>))]
        [InlineData(true, typeof(HashMap<string, string>))]
        [InlineData(true, typeof(LinkedHashMap<string, string>))]
        [InlineData(true, typeof(LinkedOrderedDictionary<string, string>))]
        [InlineData(true, typeof(ListOrderedDictionary<string, string>))]
        public void Test_CanConvert(bool expected, Type type)
        {
            var factory = new DictionaryJsonConverterFactory();
            var actual = factory.CanConvert(type);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_CreateConverter()
        {
            var factory = new DictionaryJsonConverterFactory();
            var actual = factory.CreateConverter(typeof(LinkedHashMap<string, string>), new JsonSerializerOptions());
            Assert.IsType<DictionaryJsonConverter<string, string>>(actual);
        }

        [Fact]
        public void Test_CreateConverter_argTest()
        {
            var factory = new StubFactory();
            var options = new JsonSerializerOptions();
            var actual = (StubConverter<string, int>)factory.CreateConverter(typeof(LinkedHashMap<string, int>), options);

            Assert.True(object.ReferenceEquals(options, actual.CtorArg_Options));
            Assert.True(actual.CtorArg_genericType == typeof(LinkedHashMap<,>));
        }

        class StubFactory : DictionaryJsonConverterFactory
        {
            protected override Type MakeConverterType(Type keyType, Type valueType)
            {
                return typeof(StubConverter<string, int>);
            }
        }

        class StubConverter<K, V> : DictionaryJsonConverter<K, V>
        {
            public JsonSerializerOptions CtorArg_Options { get; set; }
            public Type CtorArg_genericType { get; set; }
            public StubConverter(JsonSerializerOptions options, Type genericType) : base(options, genericType)
            {
                CtorArg_Options = options;
                CtorArg_genericType = genericType;
            }
        }


    }
}