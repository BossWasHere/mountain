using Mountain.Core;
using Mountain.Protocol.Packet;

namespace Mountain.Protocol
{
    public interface IClient
    {
        IClientConnection Connection { get; }

        ConnectionState State => Connection?.State ?? ConnectionState.Status;
        bool UseCompression => Connection?.UseCompression ?? false;

        public bool HasConnection => Connection != null;
        int ProtocolVersion { get; }
        string InitialHostname { get; }
        int InitialPort { get; }

        bool SendPacket(IOutboundPacket packet)
        {
            return Connection.SendPacket(packet);
        }

        void UpdateState(ConnectionState newState)
        {
            if (Connection != null) Connection.SetState(newState);
        }

        void SetUseCompression(bool useCompression)
        {
            if (Connection != null) Connection.SetUseCompression(useCompression);
        }

        public void UpdateRemote(int protocolVersion, string hostname, int port);
    }
}
