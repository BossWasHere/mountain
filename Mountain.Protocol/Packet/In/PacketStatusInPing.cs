using Mountain.Core;
using Mountain.Protocol.Packet.Out;
using System.Text;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketStatusInPing : IInboundPacket
    {
        public byte PacketId => Packets.In.Status.Ping.PacketId;

        public long Payload { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Payload = stream.ReadLong();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            clientConnection.SendPacket(new PacketStatusOutPong() { Payload = Payload });
        }
    }
}
