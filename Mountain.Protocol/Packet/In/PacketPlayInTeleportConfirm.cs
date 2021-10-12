using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInTeleportConfirm : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.TeleportConfirm.PacketId;

        public int TeleportId { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            TeleportId = stream.ReadVarInt();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
