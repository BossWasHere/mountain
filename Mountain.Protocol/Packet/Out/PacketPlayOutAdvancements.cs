using System;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutAdvancements : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.Advancements.PacketId;

        public void WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
