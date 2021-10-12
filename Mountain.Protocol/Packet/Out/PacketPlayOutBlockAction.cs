using Mountain.Core;
using Mountain.World;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutBlockAction : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.BlockAction.PacketId;

        public BlockPosition Position { get; set; }
        public byte ActionId { get; set; }
        public byte ActionParam { get; set; }
        public int BlockType { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteULong(Position.ToUInt64());
            stream.WriteByte(ActionId);
            stream.WriteByte(ActionParam);
            stream.WriteVarInt(BlockType);
        }
    }
}
