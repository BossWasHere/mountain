using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInPickItem : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.PickItem.PacketId;

        public int ActiveSlot { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            ActiveSlot = stream.ReadVarInt();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
