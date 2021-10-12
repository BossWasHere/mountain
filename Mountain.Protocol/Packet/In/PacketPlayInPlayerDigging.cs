using Mountain.Core;
using Mountain.Core.Enums;
using Mountain.World;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInPlayerDigging : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.PlayerDigging.PacketId;

        public DiggingStatus Status { get; private set; }
        public BlockPosition Position { get; private set; }
        public BlockFace BlockFace { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Status = stream.ReadEnumVarInt<DiggingStatus>();
            Position = new BlockPosition(stream.ReadULong());
            BlockFace = stream.ReadEnumVarInt<BlockFace>();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
