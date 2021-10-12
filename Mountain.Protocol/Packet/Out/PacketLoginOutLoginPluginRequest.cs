using Mountain.Core;
using System;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketLoginOutLoginPluginRequest : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Login.LoginPluginRequest.PacketId;

        public int MessageId { get; set; }
        public string Channel { get; set; }
        public byte[] Data { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            MessageId = stream.ReadVarInt(out int lenA);
            Channel = stream.ReadVarString(out int lenB);

            Data = new byte[lengthHint - lenA - lenB];
            stream.Read(Data);
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarInt(MessageId);
            stream.WriteVarString(Channel);
            stream.Write(Data);
        }
    }
}
