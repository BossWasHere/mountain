using Mountain.Core;
using Mountain.Core.Command;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutTabComplete : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.TabComplete.PacketId;

        public int TransactionId { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public int Count { get; set; }
        public TabCompleteMatch[] Matches { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarInt(TransactionId);
            stream.WriteVarInt(Start);
            stream.WriteVarInt(Length);

            if (Matches == null || Matches.Length < 1)
            {
                stream.WriteVarInt(0);
            }
            else
            {
                stream.WriteVarInt(Matches.Length);
                foreach (var match in Matches)
                {
                    stream.WriteVarString(match.Insert);
                    stream.WriteBool(match.HasTooltip);
                    if (match.HasTooltip) stream.Write(match.Tooltip.SerializeToUtf8Bytes());
                }
            }
        }
    }
}
