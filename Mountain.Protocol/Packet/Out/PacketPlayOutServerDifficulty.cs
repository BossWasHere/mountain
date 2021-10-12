using Mountain.Core;
using Mountain.Core.Enums;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutServerDifficulty : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Play.ServerDifficulty.PacketId;

        public Difficulty Difficulty { get; set; }
        public bool Locked { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Difficulty = stream.ReadEnumByte<Difficulty>();
            Locked = stream.ReadBool();
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteEnumByte(Difficulty);
            stream.WriteBool(Locked);
        }
    }
}
