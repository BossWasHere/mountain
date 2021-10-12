using Mountain.Core;
using Mountain.Core.Enums;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInResourcePackStatus : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.ResourcePackStatus.PacketId;

        public ResourcePackStatus Status { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Status = stream.ReadEnumVarInt<ResourcePackStatus>();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
