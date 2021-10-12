using Mountain.Core;
using Mountain.Core.Enums;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInInteractEntity : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.InteractEntity.PacketId;

        public int EntityId { get; private set; }
        public InteractEntityType Interaction { get; private set; }
        public float TargetX { get; private set; }
        public float TargetY { get; private set; }
        public float TargetZ { get; private set; }
        public Hand Hand { get; private set; }
        public bool IsSneaking { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            EntityId = stream.ReadVarInt();
            Interaction = stream.ReadEnumVarInt<InteractEntityType>();
            if (Interaction == InteractEntityType.InteractAt)
            {
                TargetX = stream.ReadFloat();
                TargetY = stream.ReadFloat();
                TargetZ = stream.ReadFloat();
            }
            Hand = Interaction != InteractEntityType.Attack ? stream.ReadEnumVarInt<Hand>() : default;
            IsSneaking = stream.ReadBool();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
