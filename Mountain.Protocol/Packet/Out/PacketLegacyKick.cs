using Mountain.Core;
using System.Text;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketLegacyKick : IOutboundPacket
    {
        private const string StringHeader = "§1\0";
        private const char NullChar = '\0';

        public byte PacketId => Packets.Out.LegacyKick.PacketId;
        public int ProtocolVersion { get; set; }
        public string ServerVersion { get; set; }
        public string MOTD { get; set; }
        public int CurrentPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public bool NeverCompress => true;

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            var sb = new StringBuilder(StringHeader);
            sb.Append(ProtocolVersion).Append(NullChar).Append(DataValidation.AssertNotNull(ServerVersion)).Append(NullChar)
            .Append(DataValidation.AssertNotNull(MOTD)).Append(NullChar).Append(CurrentPlayers).Append(NullChar).Append(MaxPlayers);

            var str = sb.ToString();
            stream.WriteShort((short)str.Length);
            stream.Write(Encoding.BigEndianUnicode.GetBytes(str));
        }
    }
}
