using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInCraftRecipeRequest : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.CraftRecipeRequest.PacketId;

        public byte WindowId { get; private set; }
        public string Recipe { get; private set; }
        public bool ShiftCraft { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            WindowId = stream.ReadByteSafe();
            Recipe = stream.ReadVarString();
            ShiftCraft = stream.ReadBool();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
