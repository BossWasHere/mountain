using Mountain.Core;
using Mountain.World;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInUpdateJigsawBlock : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.UpdateJigsawBlock.PacketId;

        public BlockPosition Position { get; private set; }
        public string Name { get; private set; }
        public string Target { get; private set; }
        public string Pool { get; private set; }
        public string FinalState { get; private set; }
        public string JointType { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Position = new BlockPosition(stream.ReadULong());
            Name = stream.ReadVarString();
            Target = stream.ReadVarString();
            Pool = stream.ReadVarString();
            FinalState = stream.ReadVarString();
            JointType = stream.ReadVarString();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
