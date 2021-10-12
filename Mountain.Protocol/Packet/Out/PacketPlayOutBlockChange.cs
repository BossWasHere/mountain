using Mountain.Core;
using Mountain.World;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutBlockChange : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.BlockChange.PacketId;

        public BlockPosition Position { get; set; }
        public int BlockId { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteULong(Position.ToUInt64());
            stream.WriteVarInt(BlockId);
        }
    }
}
