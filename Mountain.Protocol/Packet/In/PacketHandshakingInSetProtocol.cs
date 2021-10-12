using Mountain.Core;
using Mountain.Core.Enums;
using Mountain.Protocol.Packet.Out;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.In
{
    public class PacketHandshakingInSetProtocol : IInboundPacket
    {
        public byte PacketId => Packets.In.Handshaking.SetProtocol.PacketId;

        public bool IsRequestPacket { get; private set; }
        public int ProtocolVersion { get; private set; }
        public string Hostname { get; private set; }
        public int Port { get; private set; }
        public HandshakeStatus NextState { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            if (lengthHint > 1)
            {
                IsRequestPacket = false;
                ProtocolVersion = stream.ReadVarInt();
                Hostname = stream.ReadVarString();
                Port = stream.ReadUShort();
                NextState = stream.ReadEnumVarInt<HandshakeStatus>();
            }
            else
            {
                IsRequestPacket = true;
                NextState = HandshakeStatus.Status;
            }
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            if (IsRequestPacket)
            {
                var packet = new PacketStatusOutServerResponse()
                {
                    JsonMessage = manager.ServerManager.MOTDProvider.GetServerStatusMessage(manager.ServerManager.OnlinePlayers, manager.ServerManager.MaxPlayers)
                };
                clientConnection.SendPacket(packet);
            }
            else if (NextState == HandshakeStatus.Login)
            {
                var client = manager.GetOrCreateClient(clientConnection, ConnectionState.Login);
                client.UpdateRemote(ProtocolVersion, Hostname, Port);
            }
        }
    }
}
