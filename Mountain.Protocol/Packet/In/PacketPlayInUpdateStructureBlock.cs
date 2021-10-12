using Mountain.Core;
using Mountain.Core.Enums;
using Mountain.World;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInUpdateStructureBlock : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.UpdateStructureBlock.PacketId;

        public BlockPosition Position { get; private set; }
        public StructureBlockAction Action { get; private set; }
        public StructureBlockMode Mode { get; private set; }
        public string Name { get; private set; }
        public byte OffsetX { get; private set; }
        public byte OffsetY { get; private set; }
        public byte OffsetZ { get; private set; }
        public byte SizeX { get; private set; }
        public byte SizeY { get; private set; }
        public byte SizeZ { get; private set; }
        public StructureMirrorMode MirrorMode { get; private set; }
        public StructureRotationMode RotationMode { get; private set; }
        public string Metadata { get; private set; }
        public float Integrity { get; private set; }
        public long Seed { get; private set; }
        public bool IgnoreEntities { get; private set; }
        public bool ShowAir { get; private set; }
        public bool ShowBoundingBox { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Position = new BlockPosition(stream.ReadULong());
            Action = stream.ReadEnumVarInt<StructureBlockAction>();
            Mode = stream.ReadEnumVarInt<StructureBlockMode>();
            Name = stream.ReadVarString();
            OffsetX = stream.ReadByteSafe();
            OffsetY = stream.ReadByteSafe();
            OffsetZ = stream.ReadByteSafe();
            SizeX = stream.ReadByteSafe();
            SizeY = stream.ReadByteSafe();
            SizeZ = stream.ReadByteSafe();
            MirrorMode = stream.ReadEnumVarInt<StructureMirrorMode>();
            RotationMode = stream.ReadEnumVarInt<StructureRotationMode>();
            Metadata = stream.ReadVarString();
            Integrity = stream.ReadFloat();
            Seed = stream.ReadVarLong();
            var b = stream.ReadByteSafe();
            IgnoreEntities = (b & 0x01) == 0x01;
            ShowAir = (b & 0x02) == 0x02;
            ShowBoundingBox = (b & 0x04) == 0x04;
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
