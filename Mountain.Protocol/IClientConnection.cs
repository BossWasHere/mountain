using Mountain.Core;
using Mountain.Protocol.Packet;
using System;
using System.IO;
using System.Net;

namespace Mountain.Protocol
{
    public interface IClientConnection : IDisposable
    {
        int CompressionThreshold { get; }
        bool UseCompression { get; }
        ConnectionState State { get; }

        SocketAddress GetSocketEndpoint();

        bool SendPacket(IOutboundPacket packet)
        {
            return SendPacket(packet, UseCompression);
        }

        bool SendPacket(IOutboundPacket packet, bool compress)
        {
            var stream = new MemoryStream();
            PacketUtils.WritePacket(stream, packet, compress ? CompressionThreshold : -1);
            return SendBytes(stream.ToArray());
        }

        protected bool SendBytes(byte[] data);

        protected internal void SetCompressionThreshold(int compressionThreshold);
        protected internal void SetUseCompression(bool useCompression);
        protected internal void SetState(ConnectionState state);
    }
}
