using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mountain.Core.Chat
{
    public class EntityChatMessage : BaseChatMessage
    {
        [JsonPropertyName("selector")]
        public string EntitySelector { get; set; }

        public override byte[] SerializeToUtf8Bytes()
        {
            return SerializeToUtf8Bytes(typeof(EntityChatMessage));
        }
        public override string Serialize()
        {
            return Serialize(typeof(EntityChatMessage));
        }

        public static EntityChatMessage Deserialize(byte[] raw)
        {
            return JsonSerializer.Deserialize<EntityChatMessage>(raw);
        }

        public static EntityChatMessage DeserializeSafe(byte[] raw)
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
