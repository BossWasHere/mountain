using Mountain.Core;
using Mountain.World;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInGenerateStructure : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.GenerateStructure.PacketId;

        public BlockPosition Position { get; private set; }
        public int Levels { get; private set; }
        public bool KeepJigsaws { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Position = new BlockPosition(stream.ReadULong());
            Levels = stream.ReadVarInt();
            KeepJigsaws = stream.ReadBool();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
