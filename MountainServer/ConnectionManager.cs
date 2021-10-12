using log4net;
using Mountain.Core;
using Mountain.Protocol;
using Mountain.Protocol.Packet;
using MountainServer.Net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MountainServer
{
    public class ConnectionManager : IConnectionManager
    {
        private const int BufferSize = 1024;
        private const long ExtendedDataTimeout = 4000;
        private readonly ILog Logger = ServerLogger.Logger;

        public IServerManager ServerManager { get; }

        private readonly ConcurrentDictionary<SocketAddress, IClient> connections;

        private readonly CancellationTokenSource CancelSource = new CancellationTokenSource();
        private readonly ManualResetEvent Status = new ManualResetEvent(false);
        private readonly IPEndPoint EndPoint;
        private readonly Socket Socket;

        private bool Started = false;
        private bool Disposed = false;

        public ConnectionManager(IServerManager serverManager)
        {
            ServerManager = serverManager;

            connections = new ConcurrentDictionary<SocketAddress, IClient>();
            EndPoint = new IPEndPoint(IPAddress.Any, 25565);
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public IClient GetExistingClient(Socket socket)
        {
            return GetExistingClient(socket.RemoteEndPoint.Serialize());
        }

        public IClient GetExistingClient(SocketAddress address)
        {
            if (connections.TryGetValue(address, out IClient value))
            {
                return value;
            }
            return null;
        }

        public IClient GetOrCreateClient(IClientConnection connection, ConnectionState? state)
        {
            var sa = connection.GetSocketEndpoint();
            var remote = GetExistingClient(sa);
            if (remote == null)
            {
                remote = new Client(connection);
                connections.TryAdd(sa, remote);

            }
            remote.UpdateState(state ?? remote.State);
            return remote;
        }

        public ConnectionState GetConnectionState(SocketAddress address)
        {
            return GetExistingClient(address)?.State ?? ConnectionState.Status;
        }

        public void StartListen()
        {
            if (!Started)
            {
                Started = true;
                Socket.Blocking = false;
                Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
                Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                Socket.Bind(EndPoint);
                Socket.Listen(500);

                Task.Factory.StartNew(() => { StartAccept(); }, CancelSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current).SetTaskName("Network Thread");
            }
        }

        private void StartAccept()
        {
            while (!Disposed)
            {
                try
                {
                    Status.Reset();
                    var args = new SocketAsyncEventArgs();
                    var awaitWrapper = new AwaitSocketWrapper(args);

                    Socket.AcceptAsync(awaitWrapper);
                    awaitWrapper.OnCompleted(async () => await ReadAsync(args.AcceptSocket));

                    Status.WaitOne();
                }
                catch (Exception e)
                {
                    Logger.Warn(e);
                }
            }
        }

        private async Task ReadAsync(Socket socket)
        {
            Status.Set();
            if (Disposed) return;

            var args = new SocketAsyncEventArgs();
            args.SetBuffer(new byte[BufferSize], 0, BufferSize);
            var awaitWrapper = new AwaitSocketWrapper(args);

            var connection = GetExistingClient(socket)?.Connection ?? new ClientConnection(socket, ServerManager.ServerProperties.NetworkCompressionThreshold);

            var extendingBuffer = new MemoryStream();
            var writingExtended = false;
            var expectedLength = 0;
            long timeout = 0;

            while (!Disposed && socket.Connected)
            {
                try
                {
                    await socket.ReceiveAsync(awaitWrapper);
                }
                catch
                {
                    break;
                }
                int length = args.BytesTransferred;
                if (length <= 0) break;

                if (writingExtended && timeout > DateTimeOffset.Now.ToUnixTimeMilliseconds())
                {
                    extendingBuffer.Write(args.Buffer, 0, length);
                    expectedLength -= length;

                    if (expectedLength <= 0)
                    {
                        length = (int)extendingBuffer.Length;
                        Process(connection, extendingBuffer.ToArray(), length, out int e);
                        if (e > 0)
                        {
                            Logger.Warn("Client sent too much data or badly formatted packets. This may result in a disconnect");
                            break;
                        }

                        writingExtended = false;
                    }
                }
                else
                {
                    var processed = Process(connection, args.Buffer, length, out int e);
                    writingExtended = false;

                    // This runs if the packet was too long
                    if (processed < length && e > 0)
                    {
                        timeout = DateTimeOffset.Now.ToUnixTimeMilliseconds() + ExtendedDataTimeout;
                        extendingBuffer.SetLength(0);
                        writingExtended = true;

                        expectedLength = length - processed;
                        extendingBuffer.Write(args.Buffer, processed, expectedLength);
                        expectedLength = e - expectedLength;
                    }
                }

            }
        }

        private int Process(IClientConnection connection, byte[] buffer, int length, out int dataExpected)
        {
            var pd = new PacketDeserializer();
            int processed = 0;
            int lastLength = -1;
            dataExpected = 0;

            // This runs while more data supposedly exists and something can be read
            while (processed < length && lastLength != 0 && !Disposed)
            {
                var result = pd.Deserialize(buffer, processed, connection.State, connection.UseCompression);
                if (result == DeserializeState.TooLong)
                {
                    dataExpected = pd.InitialLength;
                    return processed;
                }
                if (result != DeserializeState.Done) break;

                pd.Handle(this, connection);
                lastLength = pd.InitialLength;
                processed += lastLength;
            }

            return length;
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                Disposed = true;
                Socket?.Close();
                CancelSource.Cancel();
                CancelSource.Dispose();
            }
        }

        ~ConnectionManager()
        {
            Dispose();
        }
    }
}
