using Mountain.Core;
using Mountain.Protocol;
using System;
using System.Net;
using System.Net.Sockets;

namespace MountainServer.Net
{
    public class ClientConnection : IClientConnection
    {
        public Socket Socket { get; private set; }
        public EndPoint SocketEndPoint { get; }
        public int CompressionThreshold { get; internal set; }
        public bool UseCompression { get; internal set; }
        public ConnectionState State { get; internal set; }

        public ClientConnection(Socket socket, int compressionThreshold, ConnectionState state = ConnectionState.Status, bool useCompression = false)
        {
            Socket = socket ?? throw new ArgumentNullException(nameof(socket));
            SocketEndPoint = socket.RemoteEndPoint;
            CompressionThreshold = compressionThreshold;
            UseCompression = useCompression;
            State = state;
        }

        public SocketAddress GetSocketEndpoint()
        {
            return SocketEndPoint?.Serialize() ?? null;
        }

        bool IClientConnection.SendBytes(byte[] data)
        {
            if (!Socket.Connected)
            {
                try
                {
                    Socket.Connect(SocketEndPoint);
                }
                catch
                {
                    return false;
                }
            }
            Socket.SendAsync(data, SocketFlags.None);
            return true;
        }

        public void Dispose()
        {
            if (Socket != null)
            {
                if (Socket.Connected) Socket.Disconnect(true);
                Socket.Dispose();
                Socket = null;
            }
        }

        void IClientConnection.SetCompressionThreshold(int compressionThreshold)
        {
            CompressionThreshold = compressionThreshold;
        }

        void IClientConnection.SetUseCompression(bool useCompression)
        {
            UseCompression = useCompression;
        }

        void IClientConnection.SetState(ConnectionState state)
        {
            State = state;
        }
    }
}
