using Mountain.Core;
using Mountain.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutSpawnPosition : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.SpawnPosition.PacketId;

        public BlockPosition Location {  get; set; }
        public float Angle { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteULong(Location.ToUInt64());
            stream.WriteFloat(Angle);
        }
    }
}
