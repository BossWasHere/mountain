using System.Text.Json.Serialization;

namespace Mountain.Mojang
{
    public struct ServerStatusMessage
    {
        [JsonPropertyName("version")]
        public VersionData Version { get; set; }
        [JsonPropertyName("players")]
        public PlayerData Players { get; set; }
        [JsonPropertyName("description")]
        public DescriptionData Description { get; set; }
        [JsonPropertyName("favicon")]
        public string FaviconData { get; set; }

        public struct VersionData
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("protocol")]
            public int Protocol { get; set; }
        }

        public struct PlayerData
        {
            [JsonPropertyName("max")]
            public int MaxPlayers { get; set; }
            [JsonPropertyName("online")]
            public int OnlinePlayers { get; set; }
            [JsonPropertyName("sample")]
            public PlayerNameIdPair[] PlayerSample { get; set; }
        }

        public struct DescriptionData
        {
            [JsonPropertyName("text")]
            public string Text { get; set; }
        }
    }
}
