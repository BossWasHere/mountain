using Mountain.Core;
using Mountain.Core.Enums;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInEditBook : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.EditBook.PacketId;

        public SlotData NewBookData { get; private set; }
        public bool IsSigning { get; private set; }
        public Hand Hand { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            //TODO implement slot data
            //NewBookData = stream.ReadSlotData();
            IsSigning = stream.ReadBool();
            Hand = stream.ReadEnumVarInt<Hand>();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
