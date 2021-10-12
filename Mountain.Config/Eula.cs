namespace Mountain.Config
{
    public class Eula : IniFileSettings<Eula>
    {
        private string filePath;
        public override string Comment => "#By changing the setting below to TRUE you are indicating your agreement to the EULA set out by Mojang (https://account.mojang.com/documents/minecraft_eula).";

        [DataField("eula")]
        public bool EulaAccepted { get; set; }

        public Eula()
        {
            Init();
        }

        public Eula(string eulaPath)
        {
            filePath = eulaPath;
            if (!Init(eulaPath))
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

        public bool Save(string eulaPath)
        {
            Write(ToDictionary(), eulaPath);
            return true;
        }
    }
}
