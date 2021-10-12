using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInCreativeInventoryAction : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.CreativeInventoryAction.PacketId;

        public short Slot { get; private set; }
        public SlotData Item { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Slot = stream.ReadShort();

            //TODO implement slot data
            //Item = stream.ReadSlotData();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
