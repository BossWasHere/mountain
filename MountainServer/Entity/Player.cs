using Mountain.Core;
using MountainServer.Net;

namespace MountainServer.Entity
{
    class Player : HumanEntity
    {
        public Uuid UUID { get; }
        public string Username { get; }
        public Client Handle { get; }
    }
}
