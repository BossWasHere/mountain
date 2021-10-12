using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MountainServer.Net
{
    public sealed class AwaitSocketWrapper : INotifyCompletion
    {
        private readonly static Action NONE = () => { };

        internal readonly SocketAsyncEventArgs eventArgs;
        internal Action continuation;

        public bool IsCompleted { get; internal set; }

        public AwaitSocketWrapper GetAwaiter()
        {
            return this;
        }

        public AwaitSocketWrapper(SocketAsyncEventArgs eventArgs)
        {
            this.eventArgs = eventArgs ?? throw new ArgumentNullException(nameof(eventArgs));
            this.eventArgs.Completed += delegate
            {
                (continuation ?? Interlocked.CompareExchange(ref continuation, NONE, null))?.Invoke();
            };
        }

        internal void Reset()
        {
            IsCompleted = false;
            continuation = null;
        }

        public void OnCompleted(Action action)
        {
            if (continuation == NONE || Interlocked.CompareExchange(ref continuation, action, null) == NONE)
            {
                Task.Run(action);
            }
        }

        public void GetResult()
        {
            if (eventArgs.SocketError != SocketError.Success) throw new SocketException((int)eventArgs.SocketError);
        }
    }
}
