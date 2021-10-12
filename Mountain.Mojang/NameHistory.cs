using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Mountain.Mojang
{
    public struct NameHistory
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("changedToAt")]
        [DefaultValue(long.MinValue)]
        public long ChangedToAt { get; set; }

    }
}
