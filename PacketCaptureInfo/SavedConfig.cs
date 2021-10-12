using Mountain.Config;
using Mountain.Config.Predicate;

namespace PacketCaptureInfo
{
    class SavedConfig : IniFileSettings<SavedConfig>
    {
        private readonly string filePath;

        public override string Comment => "#PacketCaptureInfo Settings";

        [DataField("deviceCount")]
        public int DeviceCount { get; set; }
        [DataField("device")]
        public string DeviceName { get; set; }
        [DataField("address")]
        public string Address { get; set; }
        [DataField("port", 25565)]
        [IntBoundsPredicate(1, 65535)]
        public int Port { get; set; }
        [DataField("filterLoopback", false)]
        public bool FilterLoopback { get; set; }
        [DataField("autoStart", false)]
        public bool AutoStart { get; set; }

        public SavedConfig()
        {
            Init();
        }

        public SavedConfig(string path)
        {
            filePath = path;
            if (!Init(path))
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
