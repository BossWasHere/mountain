using Mountain.Core;
using Mountain.Core.Enums;
using Mountain.World;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutSpawnPainting : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.SpawnPainting.PacketId;

        public int EntityId { get; set; }
        public Uuid EntityUuid { get; set; }
        public Painting Painting { get; set; }
        public BlockPosition Position { get; set; }
        public LateralDiretion Direction { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarInt(EntityId);
            stream.WriteUuid(EntityUuid);
            stream.WriteEnumVarInt(Painting);
            stream.WriteULong(Position.ToUInt64());
            stream.WriteEnumByte(Direction);
        }
    }
}
