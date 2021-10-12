using Mountain.Core;
using Mountain.Core.Chat;
using System.Text;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketLoginOutDisconnect : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Login.Disconnect.PacketId;
        public ChatMessage Message { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            var data = new byte[lengthHint];
            stream.Read(data);
            Message = ChatMessage.DeserializeSafe(data);
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.Write(Message.SerializeToUtf8Bytes());
        }
    }
}
