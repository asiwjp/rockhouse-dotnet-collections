using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries.Json.SystemTextJson
{
    internal class DictionaryJsonConverter<K, V> : JsonConverter<IDictionary<K, V>>
    {
        public static readonly Type[] SupportedKeyTypes = new Type[]
        {
            typeof(bool),
            typeof(sbyte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(byte),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(char),
            typeof(string),

            // TODO
            //typeof(DateTime),
            //typeof(DateTimeOffset),
            //typeof(Enum),
            //typeof(Guid),
            //typeof(Uri),
        };
        private readonly Type _genericType;
        private readonly JsonConverter<K> _keyConverter;
        private readonly JsonConverter<string> _stringConverter;
        private readonly JsonConverter<V?> _valueConverter;
        private readonly Type _keyType;
        private readonly Type _valueType;

        public DictionaryJsonConverter(JsonSerializerOptions options, Type genericType)
        {
            _genericType = genericType;
            _keyType = typeof(K);
            _valueType = typeof(V);
            if (!SupportedKeyTypes.Contains(_keyType))
            {
                throw new NotSupportedException($"The specified key type is not supported. type={_keyType.FullName}");
            }

            _keyConverter = (JsonConverter<K>)options.GetConverter(typeof(K));
            _valueConverter = (JsonConverter<V>)options.GetConverter(typeof(V));

            // for old version of System.Text.Json
            if (_valueConverter == null)
            {
                throw new NotSupportedException();
            }
        }

        public override IDictionary<K, V> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var col = (IDictionary<K, V>)Activator.CreateInstance(
                _genericType.MakeGenericType(new Type[] { _keyType, _valueType }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null)!;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return col;
                }

                // read key
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                K? key = (K?)Convert.ChangeType(reader.GetString(), _keyType);
                if (key == null)
                {
                    throw new JsonException($"Null key detected.");
                }

                // read value
                if (!reader.Read())
                {
                    throw new JsonException();
                }
                var value = _valueConverter.Read(ref reader, _valueType, options);
                col.Add(key, value);
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<K, V> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (var entry in value)
            {
                var propName = entry.Key.ToString();
                writer.WritePropertyName(propName);
                _valueConverter.Write(writer, entry.Value, options);
            }
            writer.WriteEndObject();
        }
    }

}


