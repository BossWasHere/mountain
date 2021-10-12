using Mountain.Core;
using System;
using System.Text;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketLoginOutEncryptionRequest : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Login.EncryptionRequest.PacketId;

        //Appears to be unused
        //public string ServerId { get; set; }
        public byte[] PubKey { get; set; }
        public byte[] VerifyToken { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            stream.Seek(20, SeekOrigin.Current);

            var pubKeyLen = stream.ReadVarInt();
            PubKey = new byte[pubKeyLen];
            stream.Read(PubKey);

            var verifyTokenLen = stream.ReadVarInt();
            VerifyToken = new byte[verifyTokenLen];
            stream.Read(VerifyToken);
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);

            // Older protocol length 21??
            //byte[] sid = new byte[21];
            //sid[0] = 0x14;
            byte[] sid = new byte[20];
            stream.Write(sid);

            stream.WriteVarInt(PubKey.Length);
            stream.Write(PubKey);
            stream.WriteVarInt(VerifyToken.Length);
            stream.Write(VerifyToken);
        }
    }
}
