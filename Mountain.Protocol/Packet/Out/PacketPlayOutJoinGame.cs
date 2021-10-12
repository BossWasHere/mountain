using Mountain.Core;
using Mountain.Core.Enums;
using Mountain.Protocol.NBT;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutJoinGame : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Play.JoinGame.PacketId;

        public int EntityId { get; set; }
        public bool Hardcore { get; set; }
        public Gamemode Gamemode { get; set; }
        public byte PreviousGamemode { get; set; }
        public int WorldCount => WorldNames.Length;
        public string[] WorldNames { get; set; }
        public NBTTag DimensionCodec { get; set; }
        public NBTTag Dimension { get; set; }
        public string WorldName { get; set; }
        public long SeedHash { get; set; }
        public int MaxPlayers { get; set; }
        public int ViewDistance { get; set; }
        public bool ReducedDebugInfo { get; set; }
        public bool EnableRespawnScreen { get; set; }
        public bool IsDebug { get; set; }
        public bool IsFlat { get; set; }


        public void ReadFromStream(Stream stream, int lengthHint)
        {
            EntityId = stream.ReadInt();
            Hardcore = stream.ReadBool();
            Gamemode = stream.ReadEnumByte<Gamemode>();
            PreviousGamemode = stream.ReadByteSafe();
            WorldNames = stream.ReadVarIntPrefixedStringArray();
            DimensionCodec = stream.ReadNextTag();
            Dimension = stream.ReadNextTag();
            WorldName = stream.ReadVarString();
            SeedHash = stream.ReadLong();
            MaxPlayers = stream.ReadVarInt();
            ViewDistance = stream.ReadVarInt();
            ReducedDebugInfo = stream.ReadBool();
            EnableRespawnScreen = stream.ReadBool();
            IsDebug = stream.ReadBool();
            IsFlat = stream.ReadBool();

        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteInt(EntityId);
            stream.WriteBool(Hardcore);
            stream.WriteEnumByte(Gamemode);
            stream.WriteByte(PreviousGamemode);
            stream.WriteVarIntPrefixedStringArray(WorldNames);
            DimensionCodec.WriteToStream(stream, false);
            Dimension.WriteToStream(stream, false);
            stream.WriteVarString(WorldName);
            stream.WriteLong(SeedHash);
            stream.WriteVarInt(MaxPlayers);
            stream.WriteVarInt(ViewDistance);
            stream.WriteBool(ReducedDebugInfo);
            stream.WriteBool(EnableRespawnScreen);
            stream.WriteBool(IsDebug);
            stream.WriteBool(IsFlat);
        }
    }
}
