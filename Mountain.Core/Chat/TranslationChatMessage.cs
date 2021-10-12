using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mountain.Core.Chat
{
    public class TranslationChatMessage : BaseChatMessage
    {
        [JsonPropertyName("translate")]
        public string Translate { get; set; }
        [JsonPropertyName("with")]
        public ChatMessage[] Placeholders { get; set; }

        public override byte[] SerializeToUtf8Bytes()
        {
            return SerializeToUtf8Bytes(typeof(TranslationChatMessage));
        }
        public override string Serialize()
        {
            return Serialize(typeof(TranslationChatMessage));
        }

        public static TranslationChatMessage Deserialize(byte[] raw)
        {
            return JsonSerializer.Deserialize<TranslationChatMessage>(raw);
        }

        public static TranslationChatMessage DeserializeSafe(byte[] raw)
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
