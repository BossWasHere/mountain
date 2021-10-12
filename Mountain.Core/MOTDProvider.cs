using Mountain.Core.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mountain.Core
{
    public class MOTDProvider
    {
        public const string DefaultFaviconB64 = "data:image/png;base64,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";

        public string MOTD { get; }

        private MOTD motdObj;
        private string currentMOTD;

        private long lastBuildMillis = 0;
        private readonly int refreshTimeMillis;

        public MOTDProvider(string serverVersion, int protocolVersion, string motd, int refreshTimeMillis = 10000)
        {
            MOTD = motd;
            motdObj = new MOTD(new MOTDVersion(serverVersion, protocolVersion), motd);
            this.refreshTimeMillis = refreshTimeMillis;
        }

        public MOTDProvider(string serverVersion, int protocolVersion, string motd, string faviconB64, int refreshTimeMillis = 10000)
        {
            MOTD = motd;
            motdObj = new MOTD(new MOTDVersion(serverVersion, protocolVersion), new MOTDPlayers(0, 1), motd, faviconB64);
            this.refreshTimeMillis = refreshTimeMillis;
        }

        public string GetServerStatusMessage(int onlinePlayers, int maxPlayers)
        {
            long current = DateTime.Now.Ticks;
            if (current - lastBuildMillis > refreshTimeMillis || currentMOTD == null)
            {
                lastBuildMillis = current;
                motdObj.Players.Online = onlinePlayers;
                motdObj.Players.Max = maxPlayers;
                currentMOTD = JsonSerializer.Serialize(motdObj);
            }

            return currentMOTD;
        }
    }

    internal class MOTD
    {
        protected static readonly JsonSerializerOptions sOpts = new JsonSerializerOptions() { IgnoreNullValues = true };

        [JsonPropertyName("version")]
        internal MOTDVersion Version { get; }
        [JsonPropertyName("players")]
        internal MOTDPlayers Players { get; }
        [JsonPropertyName("description")]
        internal ChatMessage[] Description { get; private set; }
        [JsonPropertyName("favicon")]
        internal string Favicon { get; set; }

        internal MOTD(MOTDVersion version, string unformattedDescription) : this(version, new MOTDPlayers(0, 1), unformattedDescription, MOTDProvider.DefaultFaviconB64)
        { }

        internal MOTD(MOTDVersion version, MOTDPlayers players, string unformattedDescription, string faviconB64)
        {
            Version = version;
            Players = players;
            Favicon = faviconB64;
            SetDescription(unformattedDescription);
        }

        internal void SetDescription(string unformattedDescription)
        {
            Description = ChatMessage.FromColorCodeCharString(unformattedDescription);
        }
    }

    internal class MOTDVersion
    {
        [JsonPropertyName("name")]
        internal string Name { get; }
        [JsonPropertyName("protocol")]
        internal int ProtocolVersion { get; }

        internal MOTDVersion(string name, int protocolVersion)
        {
            Name = name;
            ProtocolVersion = protocolVersion;
        }
    }

    internal class MOTDPlayers
    {
        [JsonPropertyName("online")]
        internal int Online { get; set; }
        [JsonPropertyName("max")]
        internal int Max { get; set; }

        internal MOTDPlayers(int online, int max)
        {
            Online = online;
            Max = max;
        }
    }
}
