using Mountain.Core;
using Mountain.Core.Enums;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInEntityAction : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.EntityAction.PacketId;

        public int EntityId { get; private set; }
        public EntityPlayerAction Action { get; private set; }
        public int HorseJumpBoost { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            EntityId = stream.ReadVarInt();
            Action = stream.ReadEnumVarInt<EntityPlayerAction>();
            HorseJumpBoost = stream.ReadVarInt();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
