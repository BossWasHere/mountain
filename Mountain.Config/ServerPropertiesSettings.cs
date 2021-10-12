using Mountain.Config.Predicate;
using Mountain.World;

namespace Mountain.Config
{
    public class ServerPropertiesSettings : IniFileSettings<ServerPropertiesSettings>
    {
        private readonly string filePath;

        public override string Comment => "#Minecraft server properties";

        [DataField("allow-flight")]
        public bool AllowFlight { get; set; }
        [DataField("allow-nether", true)]
        public bool AllowNether { get; set; }
        [DataField("broadcast-console-to-ops", true)]
        public bool BroadcastConsoleToOps { get; set; }
        [DataField("broadcast-rcon-to-ops", true)]
        public bool BroadcastRconToOps { get; set; }
        [DataField("difficulty", "easy")]
        public string Difficulty { get; set; }
        [DataField("enable-command-block")]
        public bool EnableCommandBlock { get; set; }
        [DataField("enable-monitoring")]
        public bool EnableMonitoring { get; set; } // Previously JMX Monitoring
        [DataField("enable-rcon")]
        public bool EnableRcon { get; set; }
        [DataField("sync-chunk-writes", true)]
        public bool SyncChunkWrites { get; set; }
        [DataField("enable-status", true)]
        public bool EnableStatus { get; set; }
        [DataField("enable-query")]
        public bool EnableQuery { get; set; }
        [DataField("enforce-whitelist")]
        public bool EnforceWhitelist { get; set; }
        [DataField("entity-broadcast-range-percentage", 100)]
        [IntBoundsPredicate(0, 500)]
        public int EntityBroadcastRangePercent { get; set; }
        [DataField("force-gamemode")]
        public bool ForceGamemode { get; set; }
        [DataField("function-permission-level")]
        [IntBoundsPredicate(1, 4)]
        public int FunctionPermissionLevel { get; set; }
        [DataField("gamemode", "survival")]
        public string Gamemode { get; set; }
        [DataField("generate-structures", true)]
        public bool GenerateStructures { get; set; }
        [DataField("generator-settings")]
        public string GeneratorSettings { get; set; }
        [DataField("hardcore")]
        public bool Hardcore { get; set; }
        [DataField("level-name", "world")]
        public string LevelName { get; set; }
        [DataField("level-seed")]
        public string LevelSeed { get; set; }
        [DataField("level-type", "default")]
        public string LevelType { get; set; }
        [DataField("max-build-height", 256)]
        [IntBoundsPredicate(0, 256)]
        [IntModuloPredicate(8, 0)] // Should be multiple of 8
        public int MaxBuildHeight { get; set; }
        [DataField("max-players", 20)]
        [IntBoundsPredicate(0, int.MaxValue)]
        public long MaxPlayers { get; set; }
        [DataField("max-tick-time", 60000)]
        [LongBoundsPredicate(0, long.MaxValue)]
        public long MaxTickTime { get; set; }
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
        [DataField("op-permission-level", 4)]
        [IntBoundsPredicate(1, 4)]
        public int OpPermissionLevel { get; set; }
        [DataField("player-idle-timeout", 0)]
        [IntBoundsPredicate(0, int.MaxValue)]
        public int PlayerIdleTimeout { get; set; }
        [DataField("prevent-proxy-connections")]
        public bool PreventProxyConnections { get; set; }
        [DataField("pvp", true)]
        public bool PVP { get; set; }
        [DataField("query.port", 25565)]
        [IntBoundsPredicate(1, 65534)]
        public int QueryPort { get; set; }
        [DataField("rate-limit", 0)]
        [IntBoundsPredicate(0, int.MaxValue)]
        public int RateLimit { get; set; }
        [DataField("rcon.password")]
        public string RconPassword { get; set; }
        [DataField("rcon.port", 25575)]
        [IntBoundsPredicate(1, 65534)]
        public int RconPort { get; set; }
        [DataField("resource-pack")]
        public string ResourcePack { get; set; }
        [DataField("resource-pack-sha1")]
        public string ResourcePackSha1 { get; set; }
        [DataField("server-ip")]
        public string ServerIp { get; set; }
        [DataField("server-port", 25565)]
        [IntBoundsPredicate(1, 65534)]
        public int ServerPort { get; set; }
        [DataField("snooper-enabled", true)]
        public bool SnooperEnabled { get; set; }
        [DataField("spawn-animals", true)]
        public bool SpawnAnimals { get; set; }
        [DataField("spawn-monsters", true)]
        public bool SpawnMonsters { get; set; }
        [DataField("spawn-npcs", true)]
        public bool SpawnNpcs { get; set; }
        [DataField("spawn-protection", 16)]
        [IntBoundsPredicate(0, int.MaxValue)]
        public int SpawnProtection { get; set; }
        [DataField("use-native-transport", true)]
        public bool UseNativeTransport { get; set; } // Linux Optimization
        [DataField("view-distance", 10)]
        [IntBoundsPredicate(3, 32)]
        public int ViewDistance { get; set; }
        [DataField("white-list")]
        public bool WhiteList { get; set; }

        public ServerPropertiesSettings()
        {
            Init();
        }

        public ServerPropertiesSettings(string serverPropertiesPath)
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
