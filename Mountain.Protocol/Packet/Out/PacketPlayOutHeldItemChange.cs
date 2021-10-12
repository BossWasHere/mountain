using Mountain.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutHeldItemChange : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Play.HeldItemChange.PacketId;

        public byte Slot { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Slot = stream.ReadByteSafe();
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteByte(Slot);
        }
    }
}
