using Mountain.Core;
using Mountain.World;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutBlockBreakAnimation : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.BlockBreakAnimation.PacketId;

        public int EntityId { get; set; }
        public BlockPosition Position { get; set; }
        public byte DestroyStage { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarInt(EntityId);
            stream.WriteULong(Position.ToUInt64());
            stream.WriteByte(DestroyStage);
        }
    }
}
