using Mountain.Core;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketLoginOutSetCompression : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Login.SetCompression.PacketId;

        public int CompressionThreshold { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            CompressionThreshold = stream.ReadVarInt();
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarInt(CompressionThreshold);
        }
    }
}
