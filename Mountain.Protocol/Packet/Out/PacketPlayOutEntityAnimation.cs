using Mountain.Core;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutEntityAnimation : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.EntityAnimation.PacketId;

        public int EntityId { get; set; }
        public byte Animation { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarInt(EntityId);
            stream.WriteByte(Animation);
        }
    }
}
