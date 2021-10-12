using Mountain.Core;
using Mountain.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutPluginMessage : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Play.PluginMessage.PacketId;

        public string Channel { get; private set; }
        public byte[] Data { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Channel = stream.ReadVarString(out int strLen);
            Data = new byte[lengthHint - strLen];
            stream.Read(Data);
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarString(Channel);
            stream.Write(Data);
        }
    }
}
