using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInClickWindowButton : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.ClickWindowButton.PacketId;

        public byte WindowId { get; private set; }
        public byte ButtonId { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            WindowId = stream.ReadByteSafe();
            ButtonId = stream.ReadByteSafe();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
