using Mountain.Protocol.Packet;

namespace MountainServer.Event
{
    public class PacketInEvent : IBaseEvent<PacketInEvent>
    {
        public static event IBaseEvent<PacketInEvent>.EventRaised OnEvent;

        public byte PacketId { get; }
        public IInboundPacket Packet { get; }

        public PacketInEvent(byte packetId, IInboundPacket packet)
        {
            PacketId = packetId;
            Packet = packet;
        }

        public IBaseEvent<PacketInEvent>.EventRaised GetHandlers()
        {
            return null;
        }
    }
}
