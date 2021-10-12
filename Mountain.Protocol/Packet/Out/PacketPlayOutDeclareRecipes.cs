using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutDeclareRecipes : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.DeclareRecipes.PacketId;

        //public Recipe[] Recipes { get; set; }

        public void WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
