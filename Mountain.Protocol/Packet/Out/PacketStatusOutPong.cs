using Mountain.Core;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketStatusOutPong : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Status.Pong.PacketId;
        public bool NeverCompress => true;

        public long Payload { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Payload = stream.ReadLong();
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteLong(Payload);
        }
    }
}
