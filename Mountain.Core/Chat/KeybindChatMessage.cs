using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mountain.Core.Chat
{
    public class KeybindChatMessage : BaseChatMessage
    {
        [JsonPropertyName("keybind")]
        public string KeybindId { get; set; }

        public override byte[] SerializeToUtf8Bytes()
        {
            return SerializeToUtf8Bytes(typeof(KeybindChatMessage));
        }
        public override string Serialize()
        {
            return Serialize(typeof(KeybindChatMessage));
        }

        public static KeybindChatMessage Deserialize(byte[] raw)
        {
            return JsonSerializer.Deserialize<KeybindChatMessage>(raw);
        }

        public static KeybindChatMessage DeserializeSafe(byte[] raw)
        {
            try
            {
                return Deserialize(raw);
            }
            catch
            {
                return default;
            }
        }
    }
}
