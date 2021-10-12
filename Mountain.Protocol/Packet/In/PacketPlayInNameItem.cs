using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInNameItem : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.NameItem.PacketId;

        public string ItemName { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            ItemName = stream.ReadVarString();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
