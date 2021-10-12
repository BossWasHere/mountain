using CommandLine;

namespace MountainServer.Env
{
    class CommandLineOptions
    {
        [Option("nogui", Required = false, HelpText = "Hide GUI")]
        public bool NoGui { get; set; }
        [Option("debuglogging", Required = false, HelpText = "Verbose debug logging")]
        public bool DebugLogging { get; set; }
        [Option("accepteula", Required = false, HelpText = "Agree to Mojang's EULA using a command line argument")]
        public bool AcceptEula { get; set; }
        [Option("properties", Required = false, Default = null, HelpText = "Set the path of the server.properties file to load")]
        public string ServerProperties { get; set; }
    }
}
