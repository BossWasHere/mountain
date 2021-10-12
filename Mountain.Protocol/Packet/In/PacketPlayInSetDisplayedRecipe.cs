using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInSetDisplayedRecipe : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.SetDisplayedRecipe.PacketId;

        public string Recipe { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Recipe = stream.ReadVarString();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
