using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInTabComplete : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.TabComplete.PacketId;

        public int TransactionId { get; private set; }
        public string ExistingText { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            TransactionId = stream.ReadVarInt();
            ExistingText = stream.ReadVarString();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
