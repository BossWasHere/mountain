using Mountain.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutPing : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.Ping.PacketId;

        public int Id { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteInt(Id);
        }
    }
}
