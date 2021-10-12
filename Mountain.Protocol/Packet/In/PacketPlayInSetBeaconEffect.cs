using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInSetBeaconEffect : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.SetBeaconEffect.PacketId;

        public int PrimaryEffectId { get; private set; }
        public int SecondaryEffectId { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            PrimaryEffectId = stream.ReadVarInt();
            SecondaryEffectId = stream.ReadVarInt();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
