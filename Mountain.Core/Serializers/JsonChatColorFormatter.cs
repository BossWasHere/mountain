using Mountain.Core.Chat;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mountain.Core.Serializers
{
    public class JsonChatColorFormatter : JsonConverter<ResolvableChatColor>
    {
        public override ResolvableChatColor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, ResolvableChatColor value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value == null ? null : value.IsStandardColor() ? value.ToJsonValueString() : value.ToHexValueString());
        }
    }
}
