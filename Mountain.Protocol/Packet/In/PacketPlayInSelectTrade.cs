using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInSelectTrade : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.SelectTrade.PacketId;

        public int SelectedSlot { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            SelectedSlot = stream.ReadVarInt();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
