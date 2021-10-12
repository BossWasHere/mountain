using Mountain.Core;
using Mountain.Core.Chat;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutChatMessage : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Play.ChatMessage.PacketId;

        public ChatMessage Message { get; set; }
        public byte Position { get; set; }
        public Uuid Sender { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            var msgLen = stream.ReadVarInt();
            var buf = new byte[msgLen];
            stream.Read(buf);
            Message = ChatMessage.DeserializeSafe(buf);

            Position = stream.ReadByteSafe();
            Sender = stream.ReadUuid();
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);

            Message.WriteVarStringUtf8Bytes(stream);
            stream.WriteByte(Position);
            stream.WriteUuid(Sender);
        }
    }
}