using Mountain.Core;
using System.Text.Json.Serialization;

namespace Mountain.Mojang
{
    public struct SessionProfile
    {
        [JsonPropertyName("name")]
        public string Username { get; set; }
        [JsonPropertyName("id")]
        public Uuid Uuid { get; set; }
        [JsonPropertyName("properties")]
        public SignedProperty[] Properties { get; set; }
        [JsonPropertyName("legacy")]
        public bool IsLegacy { get; set; }
    }
}
