using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInLockDifficulty : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.LockDifficulty.PacketId;

        public bool Locked { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Locked = stream.ReadBool();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
