using Mountain.Core;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutKeepAlive : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.KeepAlive.PacketId;

        public long KeepAliveId { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteLong(KeepAliveId);
        }
    }
}
