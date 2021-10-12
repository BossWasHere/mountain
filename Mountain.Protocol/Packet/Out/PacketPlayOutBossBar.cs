using Mountain.Core;
using Mountain.Core.Chat;
using Mountain.Core.Enums;
using System.IO;
using System.Text.Json;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutBossBar : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.BossBar.PacketId;

        public Uuid BarUuid { get; set; }
        public BossBarAction Action { get; set; }
        public ChatMessage Title { get; set; }
        public float Health { get; set; }
        public BossBarColor Color { get; set; }
        public BossBarDivision Division { get; set; }
        public bool DarkenSky { get; set; }
        public bool IsDragonBar { get; set; }
        public bool CreateFog { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            var flags = (byte)((DarkenSky ? 0x01 : 0x0) | (IsDragonBar ? 0x02 : 0x0) | (CreateFog ? 0x04 : 0x0));

            stream.WriteUuid(BarUuid);
            stream.WriteEnumVarInt(Action);
            switch (Action)
            {
                case BossBarAction.Add:
                    Title.WriteVarStringUtf8Bytes(stream);
                    stream.WriteFloat(Health);
                    stream.WriteEnumVarInt(Color);
                    stream.WriteEnumVarInt(Division);
                    stream.WriteByte(flags);
                    break;
                case BossBarAction.UpdateHealth:
                    stream.WriteFloat(Health);
                    break;
                case BossBarAction.UpdateTitle:
                    Title.WriteVarStringUtf8Bytes(stream);
                    break;
                case BossBarAction.UpdateStyle:
                    stream.WriteEnumVarInt(Color);
                    stream.WriteEnumVarInt(Division);
                    break;
                case BossBarAction.UpdateFlags:
                    stream.WriteByte(flags);
                    break;
                default:
                    break;
            }
        }
    }
}
