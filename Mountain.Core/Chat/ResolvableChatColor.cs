using Mountain.Core.Serializers;
using System.Text.Json.Serialization;

namespace Mountain.Core.Chat
{
    [JsonConverter(typeof(JsonChatColorFormatter))]
    public abstract class ResolvableChatColor
    {
        public abstract string ToJsonValueString();
        public abstract string ToHexValueString();
        public abstract bool IsStandardColor();
    }
}
