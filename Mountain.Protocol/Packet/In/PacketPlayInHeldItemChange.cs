using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInHeldItemChange : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.HeldItemChange.PacketId;

        public short HeldItemSlot { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            HeldItemSlot = stream.ReadShort();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
