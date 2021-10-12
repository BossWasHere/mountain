using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mountain.Core.Serializers
{
    public class JsonUuidFormatter : JsonConverter<Uuid>
    {
        public override Uuid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Uuid.FromString(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, Uuid value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
