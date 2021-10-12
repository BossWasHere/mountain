using Mountain.Core;
using Mountain.Core.Enums;
using Mountain.World;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInUpdateCommandBlock : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.UpdateCommandBlock.PacketId;

        public BlockPosition Position { get; private set; }
        public string Command { get; private set; }
        public CommandBlockMode Mode { get; private set; }
        public bool TrackOutput { get; private set; }
        public bool Conditional { get; private set; }
        public bool Automatic { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Position = new BlockPosition(stream.ReadULong());
            Command = stream.ReadVarString();
            Mode = stream.ReadEnumVarInt<CommandBlockMode>();
            var b = stream.ReadByteSafe();
            TrackOutput = (b & 0x01) == 0x01;
            Conditional = (b & 0x02) == 0x02;
            Automatic = (b & 0x04) == 0x04;
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
