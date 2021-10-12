using System;
using System.Collections.Generic;
using System.Text;

namespace MountainServer.Exceptions
{
    public class ServerConfigurationException : Exception
    {
        public ServerConfigurationException(string message) : base(message)
        { }
    }
}
