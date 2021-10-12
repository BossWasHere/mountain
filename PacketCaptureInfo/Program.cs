using Mountain.Core;
using Mountain.Core.Enums;
using Mountain.Protocol;
using Mountain.Protocol.Packet;
using Mountain.Protocol.Packet.In;
using Mountain.Protocol.Packet.Out;
using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PacketCaptureInfo
{
    class Program
    {
        static bool Debug = true;

        static DummyServer DummyServer = new DummyServer();

        static int Port = 25565;
        static bool FilterLoopback = false;

        const string OutFile = "packets.out";

        static bool runAppendFile = true;
        static BlockingCollection<IPacket> appendQueue = new BlockingCollection<IPacket>(new ConcurrentStack<IPacket>());

        static void Main(string[] args)
        {
            var devices = CaptureDeviceList.Instance;
            if (devices.Count < 1)
            {
                Console.WriteLine("Could not find any network devices for capture on this machine");
                return;
            }

            var settings = new SavedConfig("captureSettings.ini");
            ICaptureDevice selectedDevice = null;
            string host = "";
            bool skip = settings.AutoStart;

            if (skip)
            {
                if (devices.Count == settings.DeviceCount)
                {
                    skip = true;
                    selectedDevice = devices.FirstOrDefault(x => settings.DeviceName.Equals(x.Name));
                    if (selectedDevice == null)
                    {
                        Console.WriteLine("Device " + settings.DeviceName + " not found.");
                        skip = false;
                    }
                    else
                    {
                        host = settings.Address;
                        Port = settings.Port;
                        FilterLoopback = settings.FilterLoopback;
                        Console.WriteLine("Using saved settings");
                    }
                }
                else
                {
                    skip = false;
                    Console.WriteLine("Available device list has changed!");
                }
            }
            
            if (!skip)
            {
                Console.WriteLine("Available Devices:");
                int i = 0;
                foreach (var dev in devices)
                {
                    Console.WriteLine(i++ + " - " + dev.Description);
                }

                int devInd;
                do
                {
                    Console.WriteLine("Select a device to capture:");
                } while (!int.TryParse(Console.ReadLine(), out devInd) && devInd > 0 && devInd < devices.Count);
                selectedDevice = devices[devInd];

                Console.WriteLine("Enter server (host) IP address:");
                host = Console.ReadLine();

                do
                {
                    Console.WriteLine("Enter server listen port: (default: " + Port + ")");
                } while (!int.TryParse(Console.ReadLine(), out Port));

                Console.WriteLine("Filter loopback? (Y/n)");
                FilterLoopback = Console.ReadLine().ToLower().Equals("y");

                Console.WriteLine("Save settings and autoload? (Y/n)");
                if(Console.ReadLine().ToLower().Equals("y"))
                {
                    settings.DeviceName = selectedDevice.Name;
                    settings.DeviceCount = devices.Count;
                    settings.Address = host;
                    settings.Port = Port;
                    settings.FilterLoopback = FilterLoopback;
                    settings.AutoStart = true;
                    settings.Save();
                }
            }



            selectedDevice.OnPacketArrival += Device_OnPacketArrival;
            selectedDevice.Open(DeviceMode.Promiscuous, 1000);

            string filter = "tcp port " + Port + " and " + (FilterLoopback ? "dst host " : "host ") + host;
            selectedDevice.Filter = filter;
            Console.WriteLine("Using filter \"" + filter + '"');


            Console.WriteLine("Now capturing packets on " + selectedDevice.Description);
            selectedDevice.StartCapture();

            var cts = new CancellationTokenSource();
            var appendTask = Task.Factory.StartNew(() => StartAppendFile(cts.Token, OutFile), TaskCreationOptions.LongRunning);

            Console.WriteLine("Commands:\n - stop: Stop capturing packets\n - reset: Reset connection list");
            string cmd = "";
            do
            {
                cmd = Console.ReadLine().ToLower();
                switch (cmd)
                {
                    case "reset":
                        DummyServer.Reset();
                        break;
                    default:
                        break;
                }

            } while (!cmd.Equals("stop"));

            selectedDevice.StopCapture();
            Console.WriteLine("Closing...");

            runAppendFile = false;
            cts.Cancel();
            appendQueue.Dispose();

            appendTask.Wait(1000);

            selectedDevice.Close();
        }

        private static void Device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            var parsedPacket = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);

            var tcpPacket = parsedPacket.Extract<TcpPacket>();
            if (tcpPacket != null)
            {
                var ipPacket = (IPPacket)tcpPacket.ParentPacket;
                var srcAddr = ipPacket.SourceAddress;
                var destAddr = ipPacket.DestinationAddress;
                int srcPort = tcpPacket.SourcePort;
                int destPort = tcpPacket.DestinationPort;

                // Fake source-dest addresses for loopbacks
                if (FilterLoopback && srcPort == Port)
                {
                    srcAddr = destAddr;
                    destAddr = ipPacket.SourceAddress;
                }

                uint seq = tcpPacket.SequenceNumber;

                var payloadLen = (uint)tcpPacket.PayloadData.Length;
                var conn = DummyServer.FindConnection(srcAddr, srcPort, destAddr, destPort, out bool clientbound);
                if (conn != null)
                {
                    if (Debug) Console.WriteLine("Packet over " + conn.GetClientAddressPort() + ", Seq: " + seq + ", Len: " + payloadLen + ", Psh: " + tcpPacket.Push);
                    if (tcpPacket.Finished || tcpPacket.Reset)
                    {
                        conn.ConnectionClosed = true;
                        // Release connection
                        if (!conn.FlaggedForRelease)
                        {
                            TryAssemble(conn, true);
                            TryAssemble(conn, false);
                        }
                        DummyServer.Release(conn);
                        if (Debug) Console.WriteLine("Released connection " + conn.GetClientAddressPort());
                    }
                    // All useful packets **(should)** have ACK and payload
                    else if (conn.ThreeWayCompleted && tcpPacket.Acknowledgment && payloadLen > 0)
                    {
                        if (clientbound)
                        {
                            // Add the payload packet to the fragment set
                            conn.ClientboundFragments.Add(seq, tcpPacket);
                            if (seq == conn.WaitingServerSeq)
                            {
                                // Set the next expected sequence to the next packet we don't yet have
                                conn.ServerSeq = seq;
                                conn.WaitingServerSeq += payloadLen;
                                while (conn.ClientboundFragments.TryGetValue(conn.WaitingServerSeq, out TcpPacket np))
                                {
                                    conn.ServerSeq = np.SequenceNumber;
                                    conn.WaitingServerSeq += (uint)np.PayloadData.Length;
                                }
                            }
                            if (tcpPacket.Push)
                            {
                                // Set the sequence which we should be processing at
                                if (!conn.ServerPushing || conn.PushServerSeq > seq) conn.PushServerSeq = seq;
                                conn.ServerPushing = true;
                            }
                            if (conn.WaitingServerSeq > conn.PushServerSeq)
                            {
                                // Actually process it if we have the required packets
                                TryAssemble(conn, true);
                            }
                        }
                        else
                        {
                            // See above, but changed for serverbound
                            conn.ServerboundFragments.Add(seq, tcpPacket);
                            if (seq == conn.WaitingClientSeq)
                            {
                                conn.ClientSeq = seq;
                                conn.WaitingClientSeq += payloadLen;
                                while (conn.ServerboundFragments.TryGetValue(conn.WaitingClientSeq, out TcpPacket np))
                                {
                                    conn.ClientSeq = np.SequenceNumber;
                                    conn.WaitingClientSeq += (uint)np.PayloadData.Length;
                                }
                            }
                            if (tcpPacket.Push)
                            {
                                if (!conn.ClientPushing || conn.PushClientSeq > seq) conn.PushClientSeq = seq;
                                conn.ClientPushing = true;
                            }
                            if (conn.WaitingClientSeq > conn.PushClientSeq)
                            {
                                TryAssemble(conn, false);
                            }
                        }
                    }
                    else if (!conn.ThreeWayCompleted)
                    {
                        if (clientbound) // Handshake 2/3
                        {
                            if (tcpPacket.Synchronize && tcpPacket.Acknowledgment)
                            {
                                conn.ThreeWayState = TcpHandshakeState.SynAck;
                                conn.ServerSeq = seq;
                                conn.WaitingServerSeq = seq + 1;
                                if (Debug) Console.WriteLine("Handshake 2/3, " + conn.GetClientAddressPort());
                            }
                        }
                        else // Handshake 3/3
                        {
                            if (tcpPacket.Acknowledgment)
                            {
                                conn.ThreeWayState = TcpHandshakeState.Ack;
                                if (Debug) Console.WriteLine("Handshake 3/3, " + conn.GetClientAddressPort());
                            }
                        }
                    }
                }
                else
                {
                    if (tcpPacket.Synchronize && payloadLen == 0) // Handshake 1/3
                    {
                        conn = DummyServer.Add(srcAddr, srcPort, destAddr, destPort, seq);
                        if (Debug) Console.WriteLine("Starting handshake 1/3, " + conn.GetClientAddressPort());
                    }
                }
            }
        }

        private static void TryAssemble(Connection conn, bool clientbound)
        {
            lock (conn.AssemblyLock)
            {
                if (clientbound)
                {
                    if (!conn.ServerPushing) return;
                    conn.ServerPushing = false;
                }
                else
                {
                    if (!conn.ClientPushing) return;
                    conn.ClientPushing = false;
                }
                var ms = conn.GetMemoryStream(clientbound);
                var fragments = clientbound ? conn.ClientboundFragments : conn.ServerboundFragments;
                var lastSeq = clientbound ? conn.ServerSeq : conn.ClientSeq;

                var seqOrder = fragments.Keys.ToList();
                try
                {
                    ulong packetSeqStart = 0;
                    foreach (ulong key in seqOrder)
                    {
                        if (key > lastSeq) break;
                        if (packetSeqStart == 0) packetSeqStart = key;
                        fragments.Remove(key, out TcpPacket p);
                        ms.Write(p.PayloadData);

                        if (clientbound) conn.NextServerPacketCount++;
                        else conn.NextClientPacketCount++;

                        if (p.Push)
                        {
                            ProcessPacket(ms, conn, clientbound, packetSeqStart, clientbound ? conn.NextServerPacketCount : conn.NextClientPacketCount);
                            //ms.Position = 0;
                            ms.SetLength(0);
                            packetSeqStart = 0;

                            if (clientbound) conn.NextServerPacketCount = 0;
                            else conn.NextClientPacketCount = 0;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("A buffer error occured");
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static void ProcessPacket(MemoryStream stream, Connection conn, bool clientbound, ulong packetSeqStart, int tcpPacketCount)
        {
            var length = stream.Length;
            //Not optimal, update in future version:
            stream.Seek(0, SeekOrigin.Begin);

            //var buffer = stream.ToArray();
            if (Debug) Console.WriteLine("Processing " + length + " bytes for " + conn.GetClientAddressPort() + " (made of " + tcpPacketCount + " TCP packets)");
            var client = conn.ClientInstance;
            try
            {
                if (clientbound) // From the server
                {
                    var pd = PacketUtils.ReadClientboundPacket(stream, client.UseCompression, client.State);
                    var dss = pd.DeserializeState;
                    if (dss == DeserializeState.Done)
                    {
                        Console.WriteLine(pd.Packet.ToString());

                        if (pd.Packet is PacketLoginOutLoginSuccess)
                        {
                            Console.WriteLine("Client login, " + conn.GetClientAddressPort());
                            client.State = ConnectionState.Play;
                        }
                        else if (pd.Packet is PacketLoginOutSetCompression lsc)
                        {
                            Console.WriteLine("Client set compression (" + lsc.CompressionThreshold + "), " + conn.GetClientAddressPort());
                            client.CompressionThreshold = lsc.CompressionThreshold;
                        }
                        else if (pd.Packet is PacketLoginOutDisconnect)
                        {
                            Console.WriteLine("Client login disconnect, " + conn.GetClientAddressPort());
                            client.State = ConnectionState.Status;
                        }
                        else if (pd.Packet is PacketPlayOutDisconnect)
                        {
                            Console.WriteLine("Client play disconnect, " + conn.GetClientAddressPort());
                            client.State = ConnectionState.Status;
                        }

                        appendQueue.Add(pd.Packet);
                    }
                    else
                    {
                        Console.WriteLine("Packet unknown (starting seq: " + packetSeqStart + ", clientbound: " + client.State + "), ID: " + pd.PackedIdRead + ", Len: " + length + ", Reason: " + dss.ToString());
                    }
                }
                else // From the client
                {
                    var pd = PacketUtils.ReadPacket(stream, client.UseCompression, client.State);
                    var dss = pd.DeserializeState;
                    if (dss == DeserializeState.Done)
                    {
                        Console.WriteLine(pd.Packet.ToString());

                        if (pd.Packet is PacketHandshakingInSetProtocol hsp)
                        {
                            if (hsp.NextState == HandshakeStatus.Login)
                            {
                                client.State = ConnectionState.Login;
                            }
                        }

                        appendQueue.Add(pd.Packet);
                    }
                    else
                    {
                        Console.WriteLine("Packet unknown (starting seq: " + packetSeqStart + ", serverbound: " + client.State + "), ID: " + pd.PackedIdRead + ", Len: " + length + ", Reason: " + dss.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Malformed packet content " + (clientbound ? "from server" : "from client") + ", Len: " + length + ", starting seq: " + packetSeqStart);
                Console.WriteLine(e);
                //Console.WriteLine(BitConverter.ToString(buffer));
                Console.WriteLine();
            }
        }

        private static void StartAppendFile(CancellationToken token, string path)
        {
            string spacing = "    ";

            using (StreamWriter w = new StreamWriter(path, false))
            {
                var builder = new StringBuilder();
                while (runAppendFile)
                {
                    try
                    {
                        IPacket packet = appendQueue.Take(token);

                        builder.Append(packet.GetType().Name).AppendLine();
                        foreach (var property in packet.GetType().GetProperties())
                        {
                            builder.Append(spacing).Append(property.Name).Append(": ");

                            Type type = property.PropertyType;
                            if (type == typeof(byte[]))
                            {
                                builder.Append("[ ");
                                foreach (var b in (byte[])property.GetValue(packet))
                                {
                                    builder.Append(b.ToString("X2")).Append(" ");
                                }
                                builder.Append("]");
                            }
                            else if (type == typeof(int[]))
                            {
                                builder.Append("[ ");
                                foreach (var b in (int[])property.GetValue(packet))
                                {
                                    builder.Append(b.ToString("X8")).Append(" ");
                                }
                                builder.Append("]");
                            }
                            else if (type == typeof(long[]))
                            {
                                builder.Append("[ ");
                                foreach (var b in (long[])property.GetValue(packet))
                                {
                                    builder.Append(b.ToString("X16")).Append(" ");
                                }
                                builder.Append("]");
                            }
                            else if (type == typeof(string[]))
                            {
                                builder.Append("[ ");
                                builder.AppendJoin(' ', (string[])property.GetValue(packet));
                                builder.Append(" ]");
                            }
                            else
                            {
                                builder.Append(property.GetValue(packet));
                            }

                            builder.AppendLine();
                        }

                        w.Write(builder);
                        builder.Clear();
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
// Adapted from:
// https://www.ownedcore.com/forums/world-of-warcraft/world-of-warcraft-bots-programs/wow-memory-editing/234823-packet-reassembly-guys-who-using-sharppcap.html
