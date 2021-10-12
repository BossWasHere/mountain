using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInAdvancementTab : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.AdvancementTab.PacketId;

        public bool TabOpened { get; private set; }
        public string TabId { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            TabOpened = stream.ReadVarInt() == 0;
            if (TabOpened) TabId = stream.ReadVarString();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
