using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Sets.Json.SystemTextJson
{
    internal class SetJsonConverter<T> : JsonConverter<ISet<T>>
    {
        private readonly Type _genericType;
        private readonly Type _itemType;
        private readonly JsonConverter<T> _itemConverter;

        public SetJsonConverter(JsonSerializerOptions options, Type genericType)
        {
            _genericType = genericType;
            _itemType = typeof(T);
            _itemConverter = (JsonConverter<T>)options.GetConverter(typeof(T));

            // for old version of System.Text.Json
            if (_itemConverter == null)
            {
                throw new NotSupportedException();
            }
        }

        public override ISet<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            var col = (ISet<T>)Activator.CreateInstance(
                _genericType.MakeGenericType(new Type[] { _itemType }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null)!;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return col;
                }

                var item = _itemConverter.Read(ref reader, _itemType, options);
                if (item == null && options.IgnoreNullValues)
                {
                    continue;
                }
                col.Add(item);
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, ISet<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var item in value)
            {
                _itemConverter.Write(writer, item, options);
            }
            writer.WriteEndArray();
        }
    }

}



