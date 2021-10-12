using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInQueryEntityNBT : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.QueryEntityNBT.PacketId;

        public int TransactionId { get; private set; }
        public int EntityId { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            TransactionId = stream.ReadVarInt();
            EntityId = stream.ReadVarInt();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
