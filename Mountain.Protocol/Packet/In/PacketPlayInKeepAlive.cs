using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInKeepAlive : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.KeepAlive.PacketId;

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            throw new NotImplementedException();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
