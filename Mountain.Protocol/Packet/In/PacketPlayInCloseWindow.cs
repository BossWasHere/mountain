using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInCloseWindow : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.CloseWindow.PacketId;

        public byte WindowId { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            WindowId = stream.ReadByteSafe();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
