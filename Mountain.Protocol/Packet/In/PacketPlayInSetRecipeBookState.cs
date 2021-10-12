using Mountain.Core;
using Mountain.Core.Enums;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInSetRecipeBookState : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.SetRecipeBookState.PacketId;

        public RecipeBookInventory BookType { get; private set; }
        public bool BookOpen { get; private set; }
        public bool FilterActive { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            BookType = stream.ReadEnumVarInt<RecipeBookInventory>();
            BookOpen = stream.ReadBool();
            FilterActive = stream.ReadBool();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
