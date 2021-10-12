using Mountain.Core;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutSpawnPlayer : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.SpawnPlayer.PacketId;

        public int EntityId { get; set; }
        public Uuid PlayerUuid { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarInt(EntityId);
            stream.WriteUuid(PlayerUuid);
            stream.WriteDouble(PosX);
            stream.WriteDouble(PosY);
            stream.WriteDouble(PosZ);
            stream.WriteByte(Yaw);
            stream.WriteByte(Pitch);
        }
    }
}
