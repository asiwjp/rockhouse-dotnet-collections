using RockHouse.Collections.Dictionaries.Multi;
using RockHouse.Collections.Dictionaries.Multi.Json.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace RockHouse.Collections.Tests.Dictionaries.Multi.Json.SystemTextJson
{
    public class MultiValuedDictionaryJsonConverterFactoryTest
    {
        [Theory]
        [InlineData(false, typeof(string))]
        [InlineData(false, typeof(Dictionary<string, string>))]
        [InlineData(true, typeof(ListValuedDictionary<string, string>))]
        [InlineData(true, typeof(ListValuedMap<string, string>))]
        public void Test_CanConvert(bool expected, Type type)
        {
            var factory = new MultiValuedDictionaryJsonConverterFactory();
            var actual = factory.CanConvert(type);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_CreateConverter()
        {
            var factory = new MultiValuedDictionaryJsonConverterFactory();
            var actual = factory.CreateConverter(typeof(ListValuedDictionary<string, string>), new JsonSerializerOptions());
            Assert.IsType<MultiValuedDictionaryJsonConverter<string, string, IList<string>>>(actual);
        }

        [Fact]
        public void Test_CreateConverter_argTest()
        {
            var factory = new StubFactory();
            var options = new JsonSerializerOptions();
            var actual = (StubConverter<string, int, IList<int>>)factory.CreateConverter(typeof(ListValuedDictionary<string, int>), options);

            Assert.True(object.ReferenceEquals(options, actual.CtorArg_Options));
            Assert.True(actual.CtorArg_genericType == typeof(ListValuedDictionary<,>));
        }

        class StubFactory : MultiValuedDictionaryJsonConverterFactory
        {
            protected override Type MakeConverterType(Type collectionType, Type valueType, Type valueCollectionType)
            {
                return typeof(StubConverter<string, int, IList<int>>);
            }
        }

        class StubConverter<K, V, C> : MultiValuedDictionaryJsonConverter<K, V, C>
            where C : ICollection<V>
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