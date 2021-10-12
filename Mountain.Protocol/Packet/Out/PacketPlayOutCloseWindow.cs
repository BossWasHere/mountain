using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutCloseWindow : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.CloseWindow.PacketId;

        public byte WindowId { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteByte(WindowId);
        }
    }
}
