using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInSteerBoat : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.SteerBoat.PacketId;

        public bool LeftPaddle { get; private set; }
        public bool RightPaddle { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            LeftPaddle = stream.ReadBool();
            RightPaddle = stream.ReadBool();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
