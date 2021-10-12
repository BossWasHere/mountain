using Mountain.Core;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutUnloadChunk : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.UnloadChunk.PacketId;

        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteInt(ChunkX);
            stream.WriteInt(ChunkZ);
        }
    }
}
