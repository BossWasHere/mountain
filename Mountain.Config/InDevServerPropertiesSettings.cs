using Mountain.Config.Predicate;

namespace Mountain.Config
{
    public class InDevServerPropertiesSettings : IniFileSettings<InDevServerPropertiesSettings>
    {
        private readonly string filePath;

        public override string Comment => "#Minecraft server properties (INDEV ONLY)";

        [DataField("enable-status", true)]
        public bool EnableStatus { get; set; }
        [DataField("enable-query")]
        public bool EnableQuery { get; set; }
        [DataField("enforce-whitelist")]
        public bool EnforceWhitelist { get; set; }
        [DataField("gamemode", "survival")]
        public string Gamemode { get; set; }
        [DataField("level-name", "world")]
        public string LevelName { get; set; }
        [DataField("level-seed")]
        public string LevelSeed { get; set; }
        [DataField("level-type", "default")]
        public string LevelType { get; set; }
        [DataField("max-build-height", 256)]
        [IntBoundsPredicate(0, 256)]
        public int MaxBuildHeight { get; set; }
        [DataField("max-players", 20)]
        [IntBoundsPredicate(0, int.MaxValue)]
        public long MaxPlayers { get; set; }
        [DataField("max-world-size", World.World.MAX_WORLD_SIZE)]
        [IntBoundsPredicate(16, World.World.MAX_WORLD_SIZE)]
        public int MaxWorldSize { get; set; }
        [DataField("motd", "A Minecraft Server")]
        public string MOTD { get; set; }
        [DataField("network-compression-threshold", 256)]
        [IntBoundsPredicate(-1, int.MaxValue)]
        public int NetworkCompressionThreshold { get; set; }
        [DataField("online-mode", true)]
        public bool OnlineMode { get; set; }
        [DataField("player-idle-timeout", 0)]
        [IntBoundsPredicate(0, int.MaxValue)]
        public int PlayerIdleTimeout { get; set; }
        [DataField("query.port", 25565)]
        [IntBoundsPredicate(1, 65534)]
        public int QueryPort { get; set; }
        [DataField("rate-limit", 0)]
        [IntBoundsPredicate(0, int.MaxValue)]
        public int RateLimit { get; set; }
        [DataField("server-ip")]
        public string ServerIp { get; set; }
        [DataField("server-port", 25565)]
        [IntBoundsPredicate(1, 65534)]
        public int ServerPort { get; set; }
        [DataField("view-distance", 10)]
        [IntBoundsPredicate(3, 32)]
        public int ViewDistance { get; set; }
        [DataField("white-list")]
        public bool WhiteList { get; set; }

        public InDevServerPropertiesSettings()
        {
            Init();
        }

        public InDevServerPropertiesSettings(string serverPropertiesPath)
        {
            filePath = serverPropertiesPath;
            if (!Init(serverPropertiesPath))
            {
                Save();
            }
        }

        public bool Save()
        {
            if (filePath != null)
            {
                return Save(filePath);
            }
            return false;
        }

        public bool Save(string serverPropertiesPath)
        {
            Write(ToDictionary(), serverPropertiesPath);
            return true;
        }
    }
}
