using Mountain.Core;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketLoginOutLoginSuccess : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Login.LoginSuccess.PacketId;

        public Uuid Uuid { get; set; }
        public string Username { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Uuid = stream.ReadUuid();
            Username = stream.ReadVarString();
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteUuid(Uuid);
            stream.WriteVarString(Username);
        }
    }
}
