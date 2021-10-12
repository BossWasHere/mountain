using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInPlayerMovement : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.PlayerMovement.PacketId;

        public bool OnGround { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            OnGround = stream.ReadBool();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
