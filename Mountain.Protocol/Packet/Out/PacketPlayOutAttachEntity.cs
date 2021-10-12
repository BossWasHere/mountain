using System;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutAttachEntity : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.AttachEntity.PacketId;

        public void WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
