using Mountain.Core;
using Mountain.Core.Enums;
using Mountain.World;
using System.IO;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutAcknowlegdePlayerDigging : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.AcknowledgePlayerDigging.PacketId;

        public BlockPosition Position { get; set; }
        public int BlockStateId { get; set; }
        private DiggingStatus s;
        public DiggingStatus Status
        {
            get => s;
            set
            {
                if (value == DiggingStatus.Started || value == DiggingStatus.Cancelled || value == DiggingStatus.Finished)
                {
                    s = value;
                }
            }
        }
        public bool Successful { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteULong(Position.ToUInt64());
            stream.WriteVarInt(BlockStateId);
            stream.WriteEnumVarInt(s);
            stream.WriteBool(Successful);
        }
    }
}
