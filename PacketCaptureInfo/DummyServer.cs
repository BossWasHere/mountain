using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace PacketCaptureInfo
{
    class DummyServer
    {

        private readonly HashSet<Connection> ActiveConnections = new HashSet<Connection>();

        public DummyServer()
        { }

        public Connection Add(IPAddress clientAddr, int clientPort, IPAddress serverAddr, int serverPort, ulong clientSeq = 0)
        {
            var conn = new Connection(clientAddr, clientPort, serverAddr, serverPort, clientSeq);
            conn.WaitingClientSeq = conn.ClientSeq + 1;
            ActiveConnections.Add(conn);
            return conn;
        }

        public Connection FindConnection(IPAddress srcAddr, int srcPort, IPAddress destAddr, int destPort, out bool clientbound)
        {
            clientbound = false;

            foreach (Connection x in ActiveConnections)
            {
                if (x.Match(srcAddr, srcPort, destAddr, destPort, out clientbound))
                {
                    return x;
                }
            }

            return null;
        }

        public void Release(Connection conn)
        {
            ActiveConnections.Remove(conn);
        }

        public void Reset()
        {
            ActiveConnections.Clear();
        }
    }
}
