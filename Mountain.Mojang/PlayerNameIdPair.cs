using Mountain.Core;
using System.Text;
using System.Text.Json.Serialization;

namespace Mountain.Mojang
{
    public struct PlayerNameIdPair
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("id")]
        public Uuid Uuid { get; set; }
        [JsonPropertyName("legacy")]
        public bool IsLegacy { get; set; }
        [JsonPropertyName("demo")]
        public bool IsDemo { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("PlayerNameIdPair;");
            builder.Append("Name=").Append(Name);
            builder.Append(",Uuid=").Append(Uuid);

            return builder.ToString();
        }
    }
}
