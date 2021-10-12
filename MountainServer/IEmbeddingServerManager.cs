using Mountain.Config;
using Mountain.Protocol;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;

namespace MountainServer
{
    public interface IEmbeddingServerManager : IServerManager
    {

        public ConcurrentQueue<string> ConsoleParsingQueue { get; }
        public ManualResetEvent ConsoleParsingResetEvent { get; }

    }
}
