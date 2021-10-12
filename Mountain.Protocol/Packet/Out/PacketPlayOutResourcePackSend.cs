using Mountain.Core;
using Mountain.Core.Chat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutResourcePackSend : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.ResourcePackSend.PacketId;

        public string Url { get; set; }
        public string Hash { get; set; }
        public bool Forced { get; set; }
        public ChatMessage OptionalPromptMessage { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarString(Url);
            stream.WriteVarString(Hash);
            stream.WriteBool(Forced);
            if (OptionalPromptMessage != null)
            {
                stream.WriteBool(true);
                OptionalPromptMessage.WriteVarStringUtf8Bytes(stream);
            }
            stream.WriteBool(false);
        }
    }
}
