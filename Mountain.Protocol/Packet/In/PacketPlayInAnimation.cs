using Mountain.Core;
using Mountain.Core.Enums;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInAnimation : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.Animation.PacketId;

        public Hand Hand { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Hand = stream.ReadEnumVarInt<Hand>();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
