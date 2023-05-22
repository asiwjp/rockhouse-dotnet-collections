using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries.Json.SystemTextJson
{
    internal class DictionaryJsonConverterFactory : JsonConverterFactory
    {
        private static readonly Type[] _supportedTypes = new Type[]
        {
            typeof(HashMap<,>),
            typeof(LinkedHashMap<,>),
            typeof(LinkedOrderedDictionary<,>),
            typeof(ListOrderedDictionary<,>)
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
                Type keyType = typeToConvert.GetGenericArguments()[0];
                Type valueType = typeToConvert.GetGenericArguments()[1];
                Type converterType = this.MakeConverterType(keyType, valueType);

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

        protected virtual Type MakeConverterType(Type keyType, Type valueType)
        {
            return typeof(DictionaryJsonConverter<,>).MakeGenericType(new Type[] { keyType, valueType });
        }
    }

}
