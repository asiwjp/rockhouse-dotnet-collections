using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries.Multi.Json.SystemTextJson
{
    internal class MultiValuedDictionaryJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
            {
                return false;
            }

            Type genericType = typeToConvert.GetGenericTypeDefinition();
            return Supported.ConvertableTypes.Contains(genericType);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                Type genericType = typeToConvert.GetGenericTypeDefinition();
                Type keyType = typeToConvert.GetGenericArguments()[0];
                Type valueType = typeToConvert.GetGenericArguments()[1];
                Type valuesCollectionType = this.MakeCollectionType(typeof(IList<>), valueType);
                Type converterType = this.MakeConverterType(keyType, valueType, valuesCollectionType);

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

        protected virtual Type MakeCollectionType(Type collectionType, Type valueType)
        {
            return collectionType.MakeGenericType(new Type[] { valueType });
        }

        protected virtual Type MakeConverterType(Type keyType, Type valueType, Type valueCollectionType)
        {
            return typeof(MultiValuedDictionaryJsonConverter<,,>).MakeGenericType(new Type[] { keyType, valueType, valueCollectionType });
        }
    }

}
