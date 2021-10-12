using Mountain.Core;
using System;

namespace Mountain.Protocol
{
    public interface IConnectionManager : IDisposable
    {
        IServerManager ServerManager { get; }
        IClient GetOrCreateClient(IClientConnection connection, ConnectionState? state);

        void StartListen();
    }
}
