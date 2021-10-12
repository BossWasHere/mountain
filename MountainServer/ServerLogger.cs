using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Layout.Pattern;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using Mountain.Core;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MountainServer
{
    public static class ServerLogger
    {
        private static readonly ILoggerRepository logRepo;

        static ServerLogger()
        {
            Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepo, new FileInfo("log4net.config"));
        }

        public static ILog Logger { get; }

        internal static void SetNewLevel(Level level)
        {
            ((Hierarchy)logRepo).Root.Level = level;
            ((Hierarchy)logRepo).RaiseConfigurationChanged(EventArgs.Empty);
        }
    }
    
    // ReSharper disable once UnusedType.Global
    public class TaskNamePatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            var taskId = Task.CurrentId;
            if (taskId == null)
            {
                writer.Write(TaskAddons.MainTaskName + "/");
            }
            else
            {
                writer.Write(TaskAddons.GetTaskName((int)taskId) + "/");
            }
        }
    }
}
