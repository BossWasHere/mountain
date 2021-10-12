using Mountain.Core.Chat;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutDisconnect : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.Disconnect.PacketId;

        public ChatMessage Reason { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            Reason.WriteVarStringUtf8Bytes(stream);
        }
    }
}
