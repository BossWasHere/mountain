using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInUpdateCommandBlockMinecart : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.UpdateCommandBlockMinecart.PacketId;

        public int EntityId { get; private set; }
        public string Command { get; private set; }
        public bool TrackOutput { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            EntityId = stream.ReadVarInt();
            Command = stream.ReadVarString();
            TrackOutput = stream.ReadBool();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
