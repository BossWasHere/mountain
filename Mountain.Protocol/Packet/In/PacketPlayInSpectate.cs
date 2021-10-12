using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInSpectate : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.Spectate.PacketId;

        public Uuid TargetEntity { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            TargetEntity = stream.ReadUuid();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
