using System;
using System.Collections.Generic;
using System.Text;

namespace MountainServer.Exceptions
{
    public class ServerRunningException : Exception
    {
        public ServerRunningException(string message) : base(message)
        { }
    }
}
