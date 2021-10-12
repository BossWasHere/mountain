using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mountain.Core.Chat
{
    public class NBTChatMessage : BaseChatMessage
    {
        [JsonPropertyName("nbt")]
        public string NBTPath { get; set; }
        [JsonPropertyName("interpret")]
        public bool Interpret { get; set; }
        [JsonPropertyName("block")]
        public string BlockCoords { get; set; }
        [JsonPropertyName("entity")]
        public string EntitySelector { get; set; }
        [JsonPropertyName("storage")]
        public string NamespacedStorage { get; set; }

        public override byte[] SerializeToUtf8Bytes()
        {
            return SerializeToUtf8Bytes(typeof(NBTChatMessage));
        }
        public override string Serialize()
        {
            return Serialize(typeof(NBTChatMessage));
        }

        public static NBTChatMessage Deserialize(byte[] raw)
        {
            return JsonSerializer.Deserialize<NBTChatMessage>(raw);
        }

        public static NBTChatMessage DeserializeSafe(byte[] raw)
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
