using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInChatMessage : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.ChatMessage.PacketId;

        public string Message { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Message = stream.ReadVarString();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
