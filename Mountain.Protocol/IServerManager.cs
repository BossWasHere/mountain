using Mountain.Config;
using Mountain.Core;
using System.Net;

namespace Mountain.Protocol
{
    public interface IServerManager
    {
        public bool Active { get; }
        public bool RconEnabled { get; }
        public bool QueryEnabled { get; }
        public int ServerPort { get; }
        public IPAddress ServerIp { get; }
        public int RconPort { get; }
        public int QueryPort { get; }
        public int OnlinePlayers { get; }
        public int MaxPlayers { get; }

        public ServerPropertiesSettings ServerProperties { get; }
        public MOTDProvider MOTDProvider { get; }
        public IConnectionManager ConnectionManager { get; }

        public delegate void ServerClosedEvent();
        public event ServerClosedEvent ServerClosed;

    }
}
