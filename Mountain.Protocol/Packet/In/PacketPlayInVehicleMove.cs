using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInVehicleMove : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.VehicleMove.PacketId;

        public double PosX { get; private set; }
        public double PosY { get; private set; }
        public double PosZ { get; private set; }
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            PosX = stream.ReadDouble();
            PosY = stream.ReadDouble();
            PosZ = stream.ReadDouble();
            Yaw = stream.ReadFloat();
            Pitch = stream.ReadFloat();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
