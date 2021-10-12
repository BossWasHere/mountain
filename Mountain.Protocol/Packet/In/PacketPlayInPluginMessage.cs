using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInPluginMessage : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.PluginMessage.PacketId;

        public string Identifier { get; private set; }
        public byte[] Data { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Identifier = stream.ReadVarString(out int strLen);
            Data = new byte[lengthHint - strLen];
            stream.Read(Data);
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
