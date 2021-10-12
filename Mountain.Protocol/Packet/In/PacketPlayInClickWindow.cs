using Mountain.Core;
using Mountain.Core.Enums;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInClickWindow : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.ClickWindow.PacketId;

        public byte WindowId { get; private set; }
        public int StateId { get; private set; }
        public short Slot { get; private set; }
        public byte Button { get; private set; }
        public short ActionNumber { get; private set; }
        public InventoryInteractMode Mode { get; private set; }
        //public SlotData SlotData { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            WindowId = stream.ReadByteSafe();
            StateId = stream.ReadVarInt();
            Slot = stream.ReadShort();
            Button = stream.ReadByteSafe();
            Mode = stream.ReadEnumVarInt<InventoryInteractMode>();

            int slotLength = stream.ReadVarInt();
            //TODO implement slot data
            //SlotData = stream.ReadSlotData
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
