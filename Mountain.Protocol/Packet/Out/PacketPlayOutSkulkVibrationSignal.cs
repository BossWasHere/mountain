using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutSkulkVibrationSignal : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.SkulkVibrationSignal.PacketId;

        public void WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
