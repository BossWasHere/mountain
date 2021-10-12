using log4net;
using Mountain.Config;
using Mountain.Core;
using Mountain.Core.Chat;
using Mountain.Protocol;
using Mountain.World;
using MountainServer.Env;
using MountainServer.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MountainServer
{
    public sealed class ThreadedServerManager : IEmbeddingServerManager
    {
        private static readonly ILog Logger = ServerLogger.Logger;

        public ServerPropertiesSettings ServerProperties { get; }
        public MOTDProvider MOTDProvider { get; }
        public IConnectionManager ConnectionManager { get; }
        public WorldManager WorldManager { get; }
        public ConcurrentQueue<string> ConsoleParsingQueue { get; }
        public ManualResetEvent ConsoleParsingResetEvent { get; }

        public bool Active { get; private set; } = false;

        public bool RconEnabled { get; }
        public bool QueryEnabled { get; }
        public IPAddress ServerIp { get; }
        public int ServerPort { get; }
        public int RconPort { get; }
        public int QueryPort { get; }

        public int OnlinePlayers => 10; //PLACEHOLDER
        public int MaxPlayers => 30; //PLACEHOLDER

        private ManualResetEvent awaitCloseEvent = new ManualResetEvent(false);

        public ThreadedServerManager(ServerPropertiesSettings serverProperties)
        {
            ServerProperties = serverProperties;

            try
            {
                ServerIp = string.IsNullOrWhiteSpace(serverProperties.ServerIp) ? IPAddress.Any : IPAddress.Parse(serverProperties.ServerIp);
            }
            catch
            {
                throw new ServerConfigurationException("Invalid IP Address: " + serverProperties.ServerIp);
            }

            //DoSyncChunkWriting = serverProperties.SyncChunkWrites;
            RconEnabled = serverProperties.EnableRcon;
            QueryEnabled = serverProperties.EnableQuery;
            ServerPort = serverProperties.ServerPort;
            RconPort = serverProperties.RconPort;
            QueryPort = serverProperties.QueryPort;
            MOTDProvider = new MOTDProvider(CompileData.SubVersionIdentifier, CompileData.ProtocolVersion, $"{ChatColor.Blue}Mountain Async Server{ChatColor.Gray} - {ChatColor.Red}DO NOT CONNECT"/*ServerProperties.MOTD*/);

            ConnectionManager = new ConnectionManager(this);
            WorldManager = new WorldManager(AssemblyUtil.AssemblyDirectory);
            ConsoleParsingQueue = new ConcurrentQueue<string>();
            ConsoleParsingResetEvent = new ManualResetEvent(false);

        }

        public bool StartInstance()
        {
            if (Active) return false;
            Active = true;
            var task = Task.Factory.StartNew(() =>
            {
                StartInternal();
            }, TaskCreationOptions.LongRunning).SetTaskName("Server Thread");
            return true;
        }

        private void StartInternal()
        {
            Task.Factory.StartNew(() =>
            {
                ProcessConsoleQueue();
            }).SetTaskName("Console");

            ConnectionManager.StartListen();

            Task.Factory.StartNew(async () =>
            {
                var world = await WorldManager.LoadOrCreateWorld(ServerProperties.LevelName);
                // multiple worlds loading should be done async/await - waiting for IO disk reads anyway
            }).SetTaskName("WorldLoader");

            Thread.Sleep(1000);
            awaitCloseEvent.WaitOne();
            Logger.Info("Stopping the server...");
            Active = false;

            CloseInternal();
        }

        public bool CloseInstance()
        {
            if (!Active) return false;
            Active = false;
            CloseInternal();
            return true;
        }

        private void CloseInternal()
        {
            ConnectionManager.Dispose();
            ServerClosed?.Invoke();
        }

        public void LoadWorldData()
        {

        }

        private void ProcessConsoleQueue()
        {
            while (Active)
            {
                ConsoleParsingResetEvent.WaitOne();
                ConsoleParsingResetEvent.Reset();

                while (ConsoleParsingQueue.TryDequeue(out string value))
                {
                    switch (value.ToLower())
                    {
                        case "stop":
                            awaitCloseEvent.Set();
                            break;
                        case "tasks":
                            foreach (var task in TaskAddons.GetNamedTasks())
                            {
                                Logger.Info(task.Key + " - " + task.Value);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public event IServerManager.ServerClosedEvent ServerClosed;
    }
}
