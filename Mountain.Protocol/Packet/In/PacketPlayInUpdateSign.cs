using Mountain.Core;
using Mountain.World;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInUpdateSign : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.UpdateSign.PacketId;

        public BlockPosition Position { get; private set; }
        public string Line1 { get; private set; }
        public string Line2 { get; private set; }
        public string Line3 { get; private set; }
        public string Line4 { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Position = new BlockPosition(stream.ReadULong());
            Line1 = stream.ReadVarString();
            Line2 = stream.ReadVarString();
            Line3 = stream.ReadVarString();
            Line4 = stream.ReadVarString();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
