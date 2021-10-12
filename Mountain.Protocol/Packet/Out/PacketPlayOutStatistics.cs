using Mountain.Core;
using Mountain.Core.Statistics;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutStatistics : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.Statistics.PacketId;

        public Statistic[] Statistics { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);

            if (Statistics == null || Statistics.Length < 1)
            {
                stream.WriteByte(0);
            }
            else
            {
                stream.WriteVarInt(Statistics.Length);
                foreach (var stat in Statistics)
                {
                    stream.WriteEnumVarInt(stat.StatisticCategory);
                    stream.WriteVarInt(stat.StatisticId);
                    stream.WriteVarInt(stat.Value);
                }
            }
        }
    }
}
