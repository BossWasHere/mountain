using System.IO;

namespace Mountain.Protocol.Packet
{
    public interface IPacket
    {
        public byte PacketId { get; }
    }

    public interface IPacketDeserializable : IPacket
    {
        public void ReadFromStream(Stream stream, int lengthHint);
        //public void ReadFromStream(Stream stream, int lengthHint);
    }

    public interface IInboundPacket : IPacketDeserializable
    {
        public void Handle(IConnectionManager manager, IClientConnection clientConnection);
    }

    public interface IOutboundPacket : IPacket
    {
        public void WriteToStream(Stream stream);
        //public PacketSerializer SerializeOn();
        public bool NeverCompress => false;
    }
}
