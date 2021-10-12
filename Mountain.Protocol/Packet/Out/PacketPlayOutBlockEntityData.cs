using Mountain.Core;
using Mountain.Core.Enums;
using Mountain.Protocol.NBT;
using Mountain.World;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutBlockEntityData : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.BlockEntityData.PacketId;

        public BlockPosition Position { get; set; }
        public BlockEntityDataAction Action { get; set; }
        public NBTTag NBTTag { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteULong(Position.ToUInt64());
            stream.WriteEnumByte(Action);
            NBTTag.WriteToStream(stream, false);
        }
    }
}
