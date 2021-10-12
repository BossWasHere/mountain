﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutEntityHeadLook : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.EntityHeadLook.PacketId;

        public void WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
