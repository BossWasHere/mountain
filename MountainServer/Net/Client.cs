using Mountain.Core;
using Mountain.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace MountainServer.Net
{
    public class Client : IClient
    {
        public IClientConnection Connection { get; }

        public int ProtocolVersion { get; internal set; }
        public string InitialHostname { get; internal set; }
        public int InitialPort { get; internal set; }

        //public Player Player { get; private set; }

        public Client(IClientConnection connection)
        {
            Connection = connection;
        }

        public void UpdateRemote(int protocolVersion, string hostname, int port)
        {
            ProtocolVersion = protocolVersion;
            InitialHostname = hostname;
            InitialPort = port;
        }

        ~Client()
        {
            Connection.Dispose();
        }
    }
}
