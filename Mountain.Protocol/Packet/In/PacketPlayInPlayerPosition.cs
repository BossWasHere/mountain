using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInPlayerPosition : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.PlayerPosition.PacketId;

        public double PosX { get; private set; }
        public double PosY { get; private set; }
        public double PosZ { get; private set; }
        public bool OnGround { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            PosX = stream.ReadDouble();
            PosY = stream.ReadDouble();
            PosZ = stream.ReadDouble();
            OnGround = stream.ReadBool();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
