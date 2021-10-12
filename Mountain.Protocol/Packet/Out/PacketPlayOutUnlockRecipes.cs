using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutUnlockRecipes : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.UnlockRecipes.PacketId;

        public void WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
