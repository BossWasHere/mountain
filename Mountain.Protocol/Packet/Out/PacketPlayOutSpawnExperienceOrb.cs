using Mountain.Core;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutSpawnExperienceOrb : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.SpawnExperienceOrb.PacketId;

        public int EntityId { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }
        public short Count { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarInt(EntityId);
            stream.WriteDouble(PosX);
            stream.WriteDouble(PosY);
            stream.WriteDouble(PosZ);
            stream.WriteShort(Count);
        }
    }
}
