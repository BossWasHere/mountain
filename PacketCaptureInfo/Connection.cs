using Mountain.Core;
using PacketDotNet;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace PacketCaptureInfo
{
    class Connection
    {
        public const int MEMORY_STREAM_INIT = 256;

        public IPAddress ClientAddress;     // Client initiating the connection
        public int ClientPort;

        public IPAddress ServerAddress;     // Server receiving the connection
        public int ServerPort;

        public ulong ClientSeq;             // Current sequence sent from client
        public ulong ServerSeq;             // Current sequence sent from server

        public ulong WaitingClientSeq;      // The sequence the server is waiting for
        public ulong WaitingServerSeq;      // The sequence the client is waiting for

        public bool ClientPushing;          // Waiting to reassemble serverbound packets
        public bool ServerPushing;          // Waiting to reassemble clientbound packets

        public ulong PushClientSeq;         // Sequence for next push from client
        public ulong PushServerSeq;         // Sequence for next push from server

        public int NextClientPacketCount;   // The number of TCP packets the next serverbound MC packet is
        public int NextServerPacketCount;   // The number of TCP packets the next clientbound MC packet is

        public TcpHandshakeState ThreeWayState = TcpHandshakeState.Syn;
        public bool ThreeWayCompleted => ThreeWayState == TcpHandshakeState.Ack; // Three way handshake complete flag
        public bool ConnectionClosed { get; set; }
        public bool FlaggedForRelease => ClientboundFragments.Count == 0 && ServerboundFragments.Count == 0 && (ClientInstance.State == ConnectionState.Handshaking || ClientInstance.State == ConnectionState.Status);

        public object AssemblyLock = new object();

        public Client ClientInstance { get; set; }
        public MemoryStream ClientboundMemoryStream;
        public MemoryStream ServerboundMemoryStream;

        // Keep fragments so we can reassemble the packets
        public SortedDictionary<ulong, TcpPacket> ClientboundFragments = new SortedDictionary<ulong, TcpPacket>();
        public SortedDictionary<ulong, TcpPacket> ServerboundFragments = new SortedDictionary<ulong, TcpPacket>();

        public string GetClientAddressPort()
        {
            return string.Format("{0}:{1}", ClientAddress.ToString(), ClientPort);
        }

        public string GetServerAddressPort()
        {
            return string.Format("{0}:{1}", ServerAddress.ToString(), ServerPort);
        }

        // Check if packet is clientbound from server
        public bool IsFromServer(TcpPacket tcp, IPAddress srcAddress, IPAddress destAddress)
        {
            return IsFromServer(srcAddress, tcp.SourcePort, destAddress, tcp.DestinationPort);
        }

        public bool IsFromServer(IPAddress srcAddress, int srcPort, IPAddress destAddress, int destPort)
        {
            return ClientAddress.Equals(destAddress) && ClientPort == destPort && ServerAddress.Equals(srcAddress) && ServerPort == srcPort;
        }

        // Check if packet is serverbound from client
        public bool IsFromClient(TcpPacket tcp, IPAddress srcAddress, IPAddress destAddress)
        {
            return IsFromClient(srcAddress, tcp.SourcePort, destAddress, tcp.DestinationPort);
        }

        public bool IsFromClient(IPAddress srcAddress, int srcPort, IPAddress destAddress, int destPort)
        {
            return IsFromServer(destAddress, destPort, srcAddress, srcPort);
        }

        // Match this connection to incoming and outgoing addresses
        public bool Match(IPAddress srcAddress, int srcPort, IPAddress destAddress, int destPort, out bool clientbound)
        {
            clientbound = false;
            // Packet is clientbound from server
            if (ClientAddress.Equals(destAddress) && ClientPort == destPort && ServerAddress.Equals(srcAddress) && ServerPort == srcPort)
            {
                clientbound = true;
                return true;
            }
            // Packet is serverbound from client or not related to this connection
            return ClientAddress.Equals(srcAddress) && ClientPort == srcPort && ServerAddress.Equals(destAddress) && ServerPort == destPort;
        }

        public Connection(IPAddress clientAddress, int clientPort, IPAddress serverAddress, int serverPort, ulong clientSeq)
        {
            ClientAddress = clientAddress;
            ClientPort = clientPort;
            ServerAddress = serverAddress;
            ServerPort = serverPort;
            ClientSeq = clientSeq;

            ClientInstance = new Client();
        }

        ~Connection()
        {
            ClientboundMemoryStream?.Close();
            ServerboundMemoryStream?.Close();
        }

        public MemoryStream GetMemoryStream(bool clientbound)
        {
            if (clientbound)
            {
                if (ClientboundMemoryStream == null) ClientboundMemoryStream = new MemoryStream(MEMORY_STREAM_INIT);
                return ClientboundMemoryStream;
            }
            else
            {
                if (ServerboundMemoryStream == null) ServerboundMemoryStream = new MemoryStream(MEMORY_STREAM_INIT);
                return ServerboundMemoryStream;
            }
        }
    }
}
