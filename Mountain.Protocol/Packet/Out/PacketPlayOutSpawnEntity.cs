using Mountain.Core;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutSpawnEntity : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.SpawnEntity.PacketId;

        public int EntityId { get; set; }
        public Uuid EntityUuid { get; set; }
        public int Type { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }
        public byte Pitch { get; set; }
        public byte Yaw { get; set; }
        public int Data { get; set; }
        public short VelocityX { get; set; }
        public short VelocityY { get; set; }
        public short VelocityZ { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarInt(EntityId);
            stream.WriteUuid(EntityUuid);
            stream.WriteVarInt(Type);
            stream.WriteDouble(PosX);
            stream.WriteDouble(PosY);
            stream.WriteDouble(PosZ);
            stream.WriteByte(Pitch);
            stream.WriteByte(Yaw);
            stream.WriteInt(Data);
            stream.WriteShort(VelocityX);
            stream.WriteShort(VelocityY);
            stream.WriteShort(VelocityZ);
        }
    }
}
