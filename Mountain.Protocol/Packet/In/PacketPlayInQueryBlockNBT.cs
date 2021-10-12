using Mountain.Core;
using Mountain.World;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInQueryBlockNBT : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.QueryBlockNBT.PacketId;

        public int TransactionId { get; private set; }
        public BlockPosition Position { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            TransactionId = stream.ReadVarInt();
            Position = new BlockPosition(stream.ReadULong());
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
