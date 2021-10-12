using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mountain.Core.Chat
{
    public class ScoreChatMessage : BaseChatMessage
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("objective")]
        public string Objective { get; set; }

        public override byte[] SerializeToUtf8Bytes()
        {
            return SerializeToUtf8Bytes(typeof(ScoreChatMessage));
        }
        public override string Serialize()
        {
            return Serialize(typeof(ScoreChatMessage));
        }

        public static ScoreChatMessage Deserialize(byte[] raw)
        {
            return JsonSerializer.Deserialize<ScoreChatMessage>(raw);
        }

        public static ScoreChatMessage DeserializeSafe(byte[] raw)
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
