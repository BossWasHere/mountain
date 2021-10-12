using Mountain.Core;
using Mountain.Core.Enums;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInClientStatus : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.ClientStatus.PacketId;

        public ClientActionStatus ActionId { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            ActionId = stream.ReadEnumVarInt<ClientActionStatus>();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
