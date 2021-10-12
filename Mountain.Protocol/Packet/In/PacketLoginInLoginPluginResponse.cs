using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketLoginInLoginPluginResponse : IInboundPacket
    {
        public byte PacketId => Packets.In.Login.LoginPluginResponse.PacketId;

        public int MessageId { get; private set; }
        public bool Successful { get; private set; }
        public byte[] Data { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            MessageId = stream.ReadVarInt(out int varIntLen);
            Successful = stream.ReadBool();
            Data = new byte[lengthHint - varIntLen - 1];
            stream.Read(Data);
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
