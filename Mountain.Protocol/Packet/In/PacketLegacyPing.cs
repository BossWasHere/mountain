using Mountain.Core;
using Mountain.Protocol.Packet.Out;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.In
{
    public class PacketLegacyPing : IInboundPacket
    {
        public byte PacketId => Packets.In.Handshaking.LegacyPing.PacketId;

        public short DataLength { get; private set; }
        public byte ProtocolVersion { get; private set; }
        public short HostnameLength { get; private set; }
        public string Hostname { get; private set; }
        public int Port { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            stream.Seek(26, SeekOrigin.Current);
            DataLength = stream.ReadShort();
            ProtocolVersion = stream.ReadByteSafe();
            HostnameLength = stream.ReadShort();

            byte[] strBuf = new byte[DataLength - 7];
            stream.Read(strBuf);
            Hostname = Encoding.BigEndianUnicode.GetString(strBuf);
            Port = stream.ReadInt();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            var server = manager.ServerManager;
            clientConnection.SendPacket(new PacketLegacyKick()
            {
                CurrentPlayers = server.OnlinePlayers,
                MaxPlayers = server.MaxPlayers,
                MOTD = server.MOTDProvider.MOTD,
                ProtocolVersion = CompileData.ProtocolVersion,
                ServerVersion = CompileData.SubVersionIdentifier
            });
        }
    }
}
