using CommandLine;
using log4net;
using Mountain.Config;
using Mountain.Core;
using MountainServer.Env;
using System;
using System.Runtime.InteropServices;

namespace MountainServer
{
    internal static class Program
    {
        private static readonly ILog Logger = ServerLogger.Logger;
        private static CommandLineOptions _clOptions;
        private static ServerPropertiesSettings _serverProperties;
        private static ThreadedServerManager _mainInstance;

        static void Main(string[] args)
        {
            Logger.Info("Starting server, please wait...");
            var options = Parser.Default.ParseArguments<CommandLineOptions>(args);
            options.WithParsed(SetClOptions);

            if (_clOptions.AcceptEula)
            {
                Logger.Warn("The argument --accepteula was passed. By using this, you agree to Minecraft's EULA.");
            }
            else
            {
                var eulaConfig = new Eula("eula.txt");
                if (!eulaConfig.EulaAccepted)
                {
                    Logger.Error("You must agree to Mojang's EULA before using this. See eula.txt for more details.");
                    Environment.Exit(6);
                }

            }
            if (_clOptions.DebugLogging)
            {
                ServerLogger.SetNewLevel(log4net.Core.Level.All);
                Logger.Debug("Enabled Debug Logging");
                Logger.Debug($"Environment - Little Endian: {BitConverter.IsLittleEndian}");
            }
            try
            {
                _serverProperties = new ServerPropertiesSettings(_clOptions.ServerProperties);
            }
            catch
            {
                Logger.Error($"Error loading server properties from {_clOptions.ServerProperties}. Check the file has the correct read/write permissions");
                Environment.Exit(4);
            }

            Logger.Info($"Server is running Minecraft {CompileData.SubVersionIdentifier}, server version {CompileData.CommitVersionIdentifier}");

            _mainInstance = new ThreadedServerManager(_serverProperties);
            _mainInstance.ServerClosed += ServerClosed;
            _mainInstance.StartInstance();

            ProcessConsoleInput(_mainInstance);

            _serverProperties.Save();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CancelIoEx(IntPtr handle, IntPtr lpOverlapped);

        internal static void ServerClosed()
        {
            var handle = GetStdHandle(-10);
            CancelIoEx(handle, IntPtr.Zero);
        }

        static void ProcessConsoleInput(IEmbeddingServerManager instance)
        {
            while (instance?.Active == true)
            {
                try
                {
                    var msg = Console.ReadLine();
                    
                    instance.ConsoleParsingQueue.Enqueue(msg);
                    instance.ConsoleParsingResetEvent.Set();
                }
                catch (Exception e)
                {
                    if (e is InvalidOperationException || e is OperationCanceledException) return;
                }
            }
        }

        private static void SetClOptions(CommandLineOptions options)
        {
            _clOptions = options;
            _clOptions.ServerProperties ??= "server.properties";
        }
    }
}
