using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInSteerVehicle : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.SteerVehicle.PacketId;

        public float Horizontal { get; private set; }
        public float Forward { get; private set; }
        public bool Jump { get; private set; }
        public bool Unmount { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Horizontal = stream.ReadFloat();
            Forward = stream.ReadFloat();
            var b = stream.ReadByteSafe();
            Jump = (b & 0x01) == 0x01;
            Unmount = (b & 0x02) == 0x02;
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
