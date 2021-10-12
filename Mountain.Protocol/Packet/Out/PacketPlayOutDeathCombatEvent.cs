using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutDeathCombatEvent : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.DeathCombatEvent.PacketId;

        public void WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
