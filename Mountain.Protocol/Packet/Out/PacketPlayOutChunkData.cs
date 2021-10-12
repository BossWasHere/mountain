using Mountain.Core;
using Mountain.Protocol.NBT;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutChunkData : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Play.ChunkData.PacketId;

        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }
        //public bool IsFullChunk { get; set; }
        //public int PrimaryBitMask { get; set; }
        public long[] PrimaryBitMask { get; set; }

        public NBTTag Heightmaps { get; set; }
        public int[] Biomes { get; set; }
        public byte[] ChunkData { get; set; }
        public NBTTag[] BlockEntities { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            ChunkX = stream.ReadInt();
            ChunkZ = stream.ReadInt();
            //IsFullChunk = stream.ReadBool();
            //PrimaryBitMask = stream.ReadVarInt();
            PrimaryBitMask = stream.ReadVarIntPrefixedVarLongArray();

            Heightmaps = NBTUtils.ReadNextTag(stream);

            /*if (IsFullChunk)
            {
                Biomes = new int[stream.ReadVarInt()];
                for (int i = 0; i < Biomes.Length; i++)
                {
                    Biomes[i] = stream.ReadVarInt();
                }
            }*/
            Biomes = stream.ReadVarIntPrefixedVarIntArray();

            ChunkData = new byte[stream.ReadVarInt()];
            stream.Read(ChunkData);

            BlockEntities = new NBTTag[stream.ReadVarInt()];
            for (int i = 0; i < BlockEntities.Length; i++)
            {
                BlockEntities[i] = NBTUtils.ReadNextTag(stream);
            }
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteInt(ChunkX);
            stream.WriteInt(ChunkZ);
            //stream.WriteBool(IsFullChunk);
            //stream.WriteVarInt(PrimaryBitMask);
            stream.WriteVarIntPrefixedVarLongArray(PrimaryBitMask);

            Heightmaps.WriteToStream(stream, false);

            /*if (IsFullChunk)
            {
                if (Biomes == null)
                {
                    //What is the purpose of this?
                    Biomes = new int[1024];
                }
                stream.WriteVarInt(Biomes.Length);
                foreach (int i in Biomes)
                {
                    stream.WriteVarInt(i);
                }
            }*/

            if (Biomes == null)
            {
                //TODO What is the purpose of this?
                Biomes = new int[1024];
            }
            stream.WriteVarIntPrefixedVarIntArray(Biomes);

            stream.WriteVarInt(ChunkData.Length);
            stream.Write(ChunkData);

            if (BlockEntities == null || BlockEntities.Length < 1)
            {
                stream.WriteVarInt(0);
            }
            else
            {
                stream.WriteVarInt(BlockEntities.Length);
                foreach (var nbt in BlockEntities)
                {
                    nbt.WriteToStream(stream, false);
                }
            }
        }
    }
}
