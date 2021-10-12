using Mountain.Core;
using Mountain.Core.Enums;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInSetDifficulty : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.SetDifficulty.PacketId;

        public Difficulty Difficulty { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Difficulty = stream.ReadEnumByte<Difficulty>();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
