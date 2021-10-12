using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInPlayerAbilities : IInboundPacket
    {

        public byte PacketId => Packets.In.Play.PlayerAbilities.PacketId;

        public bool Flying { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Flying = (stream.ReadByte() & 0x02) == 0x02;
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
