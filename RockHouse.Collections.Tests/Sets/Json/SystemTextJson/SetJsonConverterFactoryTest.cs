using RockHouse.Collections.Sets;
using RockHouse.Collections.Sets.Json.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace RockHouse.Collections.Tests.Sets.Json.SystemTextJson
{
    public class SetJsonConverterFactoryTest
    {
        [Theory]
        [InlineData(false, typeof(string))]
        [InlineData(false, typeof(HashSet<string>))]
        [InlineData(true, typeof(LinkedHashSet<string>))]
        [InlineData(true, typeof(LinkedOrderedSet<string>))]
        [InlineData(true, typeof(ListOrderedSet<string>))]
        public void Test_CanConvert(bool expected, Type type)
        {
            var factory = new SetJsonConverterFactory();
            var actual = factory.CanConvert(type);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_CreateConverter()
        {
            var factory = new SetJsonConverterFactory();
            var actual = factory.CreateConverter(typeof(LinkedHashSet<string>), new JsonSerializerOptions());
            Assert.IsType<SetJsonConverter<string>>(actual);
        }

        [Fact]
        public void Test_CreateConverter_argTest()
        {
            var factory = new StubFactory();
            var options = new JsonSerializerOptions();
            var actual = (StubConverter<string>)factory.CreateConverter(typeof(LinkedHashSet<string>), options);

            Assert.True(object.ReferenceEquals(options, actual.CtorArg_Options));
            Assert.True(actual.CtorArg_genericType == typeof(LinkedHashSet<>));
        }

        class StubFactory : SetJsonConverterFactory
        {
            protected override Type MakeConverterType(Type elementType)
            {
                return typeof(StubConverter<string>);
            }
        }

        class StubConverter<T> : SetJsonConverter<T>
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