using Mountain.Core;
using System;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.In
{
    public class PacketLoginInEncryptionResponse : IInboundPacket
    {
        public byte PacketId => Packets.In.Login.EncryptionResponse.PacketId;

        public int SharedSecretLength { get; private set; }
        public byte[] SharedSecret { get; private set; }
        public int VerifyTokenLength { get; private set; }
        public byte[] VerifyToken { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            SharedSecretLength = stream.ReadVarInt();
            SharedSecret = new byte[SharedSecretLength];
            stream.Read(SharedSecret);

            VerifyTokenLength = stream.ReadVarInt();
            VerifyToken = new byte[VerifyTokenLength];
            stream.Read(VerifyToken);
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
