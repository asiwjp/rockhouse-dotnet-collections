using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries.Multi.Json.SystemTextJson
{
    internal class MultiValuedDictionaryJsonConverter<K, V, C> : JsonConverter<IMultiValuedDictionary<K, V, C>>
        where C : ICollection<V>
    {
        private readonly Type _genericType;
        private readonly JsonConverter<V?> _valueConverter;
        private readonly Type _keyType;
        private readonly Type _valueType;

        public MultiValuedDictionaryJsonConverter(JsonSerializerOptions options, Type genericType)
        {
            _genericType = genericType;
            _keyType = typeof(K);
            _valueType = typeof(V);
            Supported.CheckKeyType(_keyType);

            _valueConverter = (JsonConverter<V>)options.GetConverter(typeof(V));

            // for old version of System.Text.Json
            if (_valueConverter == null)
            {
                throw new NotSupportedException();
            }
        }

        public override IMultiValuedDictionary<K, V, C> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var col = (IMultiValuedDictionary<K, V, C>)Activator.CreateInstance(
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

                // read values
                reader.Read();
                if (reader.TokenType == JsonTokenType.Null)
                {
                    continue;
                }
                else if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException();
                }

                var endArrayDetected = false;
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        endArrayDetected = true;
                        break;
                    }

                    var value = _valueConverter.Read(ref reader, _valueType, options);
                    col.Add(key, value);
                }
                if (!endArrayDetected)
                {
                    throw new JsonException();
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, IMultiValuedDictionary<K, V, C> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (var key in value.Keys)
            {
                var propName = key.ToString();
                writer.WritePropertyName(propName);
                writer.WriteStartArray();
                foreach (var e in value[key])
                {
                    _valueConverter.Write(writer, e, options);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndObject();
        }
    }

}


