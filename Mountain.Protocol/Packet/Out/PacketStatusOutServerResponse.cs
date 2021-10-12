using Mountain.Core;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketStatusOutServerResponse : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Status.StatusResponse.PacketId;
        public bool NeverCompress => true;

        public string JsonMessage { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            JsonMessage = stream.ReadVarString();
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarString(JsonMessage);
        }
    }
}
