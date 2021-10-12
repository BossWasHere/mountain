using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutScoreboardObjective : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.ScoreboardObjective.PacketId;

        public void WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
