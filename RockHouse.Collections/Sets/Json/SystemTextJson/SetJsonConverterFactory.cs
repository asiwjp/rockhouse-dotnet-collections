using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Sets.Json.SystemTextJson
{
    internal class SetJsonConverterFactory : JsonConverterFactory
    {
        private static readonly Type[] _supportedTypes = new Type[]
        {
            typeof(LinkedHashSet<>),
            typeof(LinkedOrderedSet<>),
            typeof(ListOrderedSet<>),
        };

        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
            {
                return false;
            }

            Type genericType = typeToConvert.GetGenericTypeDefinition();
            return _supportedTypes.Contains(genericType);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                Type genericType = typeToConvert.GetGenericTypeDefinition();
                Type itemType = typeToConvert.GetGenericArguments()[0];
                Type converterType = this.MakeConverterType(itemType);

                return (JsonConverter)Activator.CreateInstance(
                    converterType,
                    BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    args: new object[] { options, genericType },
                    culture: null)!;
            }
            catch (TargetInvocationException ex)
            {
                throw new NotSupportedException($"This type is not supported. type={typeToConvert}", ex);
            }
        }

        protected virtual Type MakeConverterType(Type elementType)
        {
            return typeof(SetJsonConverter<>).MakeGenericType(new Type[] { elementType });
        }
    }

}
