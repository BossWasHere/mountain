using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInPong : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.Pong.PacketId;

        public int Id { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Id = stream.ReadInt();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
