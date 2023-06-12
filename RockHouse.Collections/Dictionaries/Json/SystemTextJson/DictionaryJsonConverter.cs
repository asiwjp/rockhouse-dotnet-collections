using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Dictionaries.Json.SystemTextJson
{
    internal class DictionaryJsonConverter<K, V> : JsonConverter<IDictionary<K, V>>
    {
        private readonly Type _genericType;
        private readonly Func<string, object> _keyParser;
        private readonly Func<object, string> _keyFormatter;
        private readonly JsonConverter<V?> _valueConverter;
        private readonly Type _keyType;
        private readonly Type _valueType;

        public DictionaryJsonConverter(JsonSerializerOptions options, Type genericType)
        {
            _genericType = genericType;
            _keyType = typeof(K);
            _valueType = typeof(V);
            Supported.CheckKeyType(_keyType);

            _keyParser = this.GetKeyParser(_keyType);
            _keyFormatter = this.GetKeyFormatter(_keyType);
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

                K key = (K)_keyParser(reader.GetString());

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
                var propName = _keyFormatter(entry.Key);
                writer.WritePropertyName(propName);
                _valueConverter.Write(writer, entry.Value, options);
            }
            writer.WriteEndObject();
        }

        private Func<string, object> GetKeyParser(Type keyType)
        {
            if (keyType == typeof(Guid))
            {
                return (value) =>
                {
                    try
                    {
                        return new Guid(value);
                    }
                    catch (OverflowException e)
                    {
                        throw new JsonException($"Could not convert Json value to type {keyType}.", e);
                    }
                    catch (FormatException e)
                    {
                        throw new JsonException($"Could not convert Json value to type {keyType}.", e);
                    }
                };
            }
            else if (keyType == typeof(Uri))
            {
                return (value) =>
                {
                    try
                    {
                        return new Uri(value);
                    }
                    catch (FormatException e)
                    {
                        throw new JsonException($"Could not convert Json value to type {keyType}.", e);
                    }
                };
            }
            else if (keyType == typeof(DateTimeOffset))
            {
                return (value) =>
                {
                    try
                    {
                        return DateTimeOffset.Parse(value);
                    }
                    catch (FormatException e)
                    {
                        throw new JsonException($"Could not convert Json value to type {keyType}.", e);
                    }
                };
            }
            else
            {
                return (value) =>
                {
                    try
                    {
                        return Convert.ChangeType(value, keyType);
                    }
                    catch (OverflowException e)
                    {
                        throw new JsonException($"Could not convert Json value to type {keyType}.", e);
                    }
                    catch (FormatException e)
                    {
                        throw new JsonException($"Could not convert Json value to type {keyType}.", e);
                    }
                };
            }
        }

        private Func<object, string> GetKeyFormatter(Type keyType)
        {
            if (keyType == typeof(DateTimeOffset))
            {
                return (value) =>
                {
                    return ((DateTimeOffset)value).ToString("O");
                };
            }
            else
            {
                return (value) =>
                {
                    return value.ToString();
                };
            }
        }
    }

}


