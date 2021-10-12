using Mountain.Core;
using Mountain.Core.Enums;
using Mountain.World;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInPlayerBlockPlacement : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.PlayerBlockPlacement.PacketId;

        public Hand Hand { get; private set; }
        public BlockPosition Position { get; private set; }
        public BlockFace BlockFace { get; private set; }
        public float CursorX { get; private set; }
        public float CursorY { get; private set; }
        public float CursorZ { get; private set; }
        public bool InsideBlock { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Hand = stream.ReadEnumVarInt<Hand>();
            Position = new BlockPosition(stream.ReadULong()); ;
            BlockFace = stream.ReadEnumVarInt<BlockFace>();
            CursorX = stream.ReadFloat();
            CursorY = stream.ReadFloat();
            CursorZ = stream.ReadFloat();
            InsideBlock = stream.ReadBool();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
