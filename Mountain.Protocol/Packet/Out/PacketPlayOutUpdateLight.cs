using Mountain.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutUpdateLight : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Play.UpdateLight.PacketId;

        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }
        public bool TrustEdges { get; set; }
        public long[] SkyLightMask { get; set; }
        public long[] BlockLightMask { get; set; }
        public long[] EmptySkyLightMask { get; set; }
        public long[] EmptyBlockLightMask { get; set; }
        public byte[][] SkyLight { get; set; }
        public byte[][] BlockLight { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            ChunkX = stream.ReadVarInt();
            ChunkZ = stream.ReadVarInt();
            TrustEdges = stream.ReadBool();
            SkyLightMask = stream.ReadVarIntPrefixedLongArray();
            BlockLightMask = stream.ReadVarIntPrefixedLongArray();
            EmptySkyLightMask = stream.ReadVarIntPrefixedLongArray();
            EmptyBlockLightMask = stream.ReadVarIntPrefixedLongArray();

            SkyLight = new byte[stream.ReadVarInt()][];
            for (int i = 0; i < SkyLight.Length; i++)
            {
                SkyLight[i] = stream.ReadVarIntPrefixedByteArray();
            }
            //TODO the data can finish here?
            BlockLight = new byte[stream.ReadVarInt()][];
            for (int i = 0; i < BlockLight.Length; i++)
            {
                BlockLight[i] = stream.ReadVarIntPrefixedByteArray();
            }
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarInt(ChunkX);
            stream.WriteVarInt(ChunkZ);
            stream.WriteBool(TrustEdges);
            stream.WriteVarIntPrefixedLongArray(SkyLightMask);
            stream.WriteVarIntPrefixedLongArray(BlockLightMask);
            stream.WriteVarIntPrefixedLongArray(EmptySkyLightMask);
            stream.WriteVarIntPrefixedLongArray(EmptyBlockLightMask);

            stream.WriteVarInt(SkyLight.Length);
            for (int i = 0; i < SkyLight.Length; i++)
            {
                stream.WriteVarIntPrefixedByteArray(SkyLight[i]);
            }
            stream.WriteVarInt(BlockLight.Length);
            for (int i = 0; i < BlockLight.Length; i++)
            {
                stream.WriteVarIntPrefixedByteArray(BlockLight[i]);
            }
        }
    }
}
