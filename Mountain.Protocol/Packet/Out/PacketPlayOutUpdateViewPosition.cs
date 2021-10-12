using Mountain.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutUpdateViewPosition : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Play.UpdateViewPosition.PacketId;

        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            ChunkX = stream.ReadVarInt();
            ChunkZ = stream.ReadVarInt();
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarInt(ChunkX);
            stream.WriteVarInt(ChunkZ);
        }
    }
}
