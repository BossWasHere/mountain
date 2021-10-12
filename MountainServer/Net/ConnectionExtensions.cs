using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MountainServer.Net
{
    public static class ConnectionExtensions
    {
        public static AwaitSocketWrapper AcceptAsync(this Socket socket, AwaitSocketWrapper awaitable)
        {
            awaitable.Reset();
            if (!socket.AcceptAsync(awaitable.eventArgs)) awaitable.IsCompleted = true;
            return awaitable;
        }

        public static AwaitSocketWrapper ReceiveAsync(this Socket socket, AwaitSocketWrapper awaitable)
        {
            awaitable.Reset();
            if (!socket.ReceiveAsync(awaitable.eventArgs)) awaitable.IsCompleted = true;
            return awaitable;
        }

        public static AwaitSocketWrapper SendAsync(this Socket socket, AwaitSocketWrapper awaitable)
        {
            awaitable.Reset();
            if (!socket.SendAsync(awaitable.eventArgs)) awaitable.IsCompleted = true;
            return awaitable;
        }
    }
}
