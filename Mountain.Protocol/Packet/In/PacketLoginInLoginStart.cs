using Mountain.Core;
using System;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.In
{
    public class PacketLoginInLoginStart : IInboundPacket
    {
        public byte PacketId => Packets.In.Login.LoginStart.PacketId;

        public string Username { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Username = stream.ReadVarString();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
