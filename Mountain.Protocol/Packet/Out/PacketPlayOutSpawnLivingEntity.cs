using Mountain.Core;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutSpawnLivingEntity : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Play.SpawnLivingEntity.PacketId;

        public int EntityId { get; set; }
        public Uuid EntityUuid { get; set; }
        public int Type { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }
        public byte Pitch { get; set; }
        public byte Yaw { get; set; }
        public byte HeadPitch { get; set; }
        public short VelocityX { get; set; }
        public short VelocityY { get; set; }
        public short VelocityZ { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            EntityId = stream.ReadVarInt();
            EntityUuid = stream.ReadUuid();
            Type = stream.ReadVarInt();
            PosX = stream.ReadDouble();
            PosY = stream.ReadDouble();
            PosZ = stream.ReadDouble();
            Yaw = stream.ReadByteSafe();
            Pitch = stream.ReadByteSafe();
            HeadPitch = stream.ReadByteSafe();
            VelocityX = stream.ReadShort();
            VelocityY = stream.ReadShort();
            VelocityZ = stream.ReadShort();
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarInt(EntityId);
            stream.WriteUuid(EntityUuid);
            stream.WriteVarInt(Type);
            stream.WriteDouble(PosX);
            stream.WriteDouble(PosY);
            stream.WriteDouble(PosZ);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
            stream.WriteByte(HeadPitch);
            stream.WriteShort(VelocityX);
            stream.WriteShort(VelocityY);
            stream.WriteShort(VelocityZ);
        }
    }
}
