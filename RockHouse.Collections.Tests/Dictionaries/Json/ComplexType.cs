using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tests.Dictionaries.Json
{
#if !NET5_0_OR_GREATER
    [JsonConverter(typeof(ComplexTypeJsonConverter))]
#endif
    public class ComplexType
    {
        public int Int { get; set; }

        public int[] IntArray { get; set; }

        public List<int> IntList { get; set; }

        public string Str { get; set; }

        public string[] StrArray { get; set; }

        public DateTimeOffset DateTimeOffset { get; set; }

        public ComplexType Complex { get; set; }

        public ComplexType() { }
    }

    public class ComplexTypeJsonConverter : JsonConverter<ComplexType>
    {
        public override ComplexType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var o = new ComplexType();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return o;
                }

                var propName = reader.GetString();

                reader.Read();
                switch (propName)
                {
                    case "Int":
                        o.Int = reader.GetInt32();
                        break;
                    case "IntArray":
                        o.IntArray = this.ReadArray<int>(ref reader, options);
                        break;
                    case "IntList":
                        o.IntList = this.ReadList<int>(ref reader, options);
                        break;
                    case "Str":
                        o.Str = reader.GetString();
                        break;
                    case "StrArray":
                        o.StrArray = this.ReadArray<string>(ref reader, options);
                        break;
                    case "DateTimeOffset":
                        o.DateTimeOffset = reader.GetDateTimeOffset();
                        break;
                    case "Complex":
                        o.Complex = this.Read<ComplexType>(ref reader, options);
                        break;
                    default:
                        throw new Exception();
                }
            }
            throw new JsonException();
        }

        private T Read<T>(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return default;
            }

            var converter = (JsonConverter<T>)options.GetConverter(typeof(T));
            return converter.Read(ref reader, typeof(T), options);
        }

        private List<T> ReadList<T>(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            return this.ReadArray<T>(ref reader, options).ToList();
        }

        private T[] ReadArray<T>(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            var converter = (JsonConverter<T>)options.GetConverter(typeof(T));

            var ary = new List<T>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return ary.ToArray();
                }
                T e = converter.Read(ref reader, typeof(T), options);
                ary.Add(e);
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, ComplexType value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteNumber("Int", value.Int);

            if (value.IntArray == null)
            {
                writer.WriteNull("IntArray");
            }
            else
            {
                writer.WriteStartArray("IntArray");
                foreach (var e in value.IntArray)
                {
                    writer.WriteNumberValue(e);
                }
                writer.WriteEndArray();
            }

            if (value.StrArray == null)
            {
                writer.WriteNull("IntList");
            }
            else
            {
                writer.WriteStartArray("IntList");
                foreach (var e in value.IntList)
                {
                    writer.WriteNumberValue(e);
                }
                writer.WriteEndArray();
            }

            writer.WriteString("Str", value.Str);

            if (value.StrArray == null)
            {
                writer.WriteNull("StrArray");
            }
            else
            {
                writer.WriteStartArray("StrArray");
                foreach (var e in value.StrArray)
                {
                    writer.WriteStringValue(e);
                }
                writer.WriteEndArray();
            }

            writer.WritePropertyName("DateTimeOffset");
            var dateTimeOffsetConverter = (JsonConverter<DateTimeOffset>)options.GetConverter(typeof(DateTimeOffset));
            dateTimeOffsetConverter.Write(writer, value.DateTimeOffset, options);

            writer.WritePropertyName("Complex");
            if (value.Complex == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                var complexConverter = (JsonConverter<ComplexType>)options.GetConverter(typeof(ComplexType));
                complexConverter.Write(writer, value.Complex, options);
            }
            writer.WriteEndObject();
        }
    }
}
