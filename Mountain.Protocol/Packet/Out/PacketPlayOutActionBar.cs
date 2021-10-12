using System;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutActionBar : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.ActionBar.PacketId;

        public void WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
