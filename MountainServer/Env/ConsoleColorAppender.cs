using log4net.Appender;
using log4net.Core;
using log4net.Util;
using Mountain.Core.Chat;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace MountainServer.Env
{
    public class ConsoleColorAppender : ManagedColoredConsoleAppender
    {
        private readonly LevelMapping levelMapping = new LevelMapping();

        public new void AddMapping(LevelColors mapping)
        {
            levelMapping.Add(new ColorsMappingWrapper(mapping));
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            levelMapping.ActivateOptions();
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            Console.ResetColor();

            var preferredForeground = Console.ForegroundColor;

            if (levelMapping.Lookup(loggingEvent.Level) is ColorsMappingWrapper withColor)
            {
                if (withColor.HasForegroundColor)
                {
                    preferredForeground = withColor.ForegroundColor;
                    Console.ForegroundColor = preferredForeground;
                }
                if (withColor.HasBackgroundColor) Console.BackgroundColor = withColor.BackgroundColor;
            }

            var renderedLayout = RenderLoggingEvent(loggingEvent);
            var parts = ChatColor.ColorMatcher.Split(renderedLayout);

            foreach (var chunk in parts)
            {
                if (chunk.StartsWith(ChatColor.ColorChar) && chunk.Length > 1)
                {
                    var chatColor = ChatColor.FromChar(chunk[1]);
                    if (chatColor.IsColor) Console.ForegroundColor = chatColor.ConsoleColor;
                    else if (!chatColor.IsFormatter) Console.ForegroundColor = preferredForeground;
                }
                else
                {
                    Console.Write(chunk);
                }
            }

            Console.ResetColor();
        }

        internal class ColorsMappingWrapper : LevelMappingEntry
        {
            public ConsoleColor ForegroundColor
            {
                get
                {
                    return foreColor;
                }
                set
                {
                    foreColor = value;
                    HasForegroundColor = true;
                }
            }

            public ConsoleColor BackgroundColor
            {
                get
                {
                    return backColor;
                }
                set
                {
                    backColor = value;
                    HasBackgroundColor = true;
                }
            }

            public bool HasForegroundColor { get; private set; }
            public bool HasBackgroundColor { get; private set; }

            private ConsoleColor foreColor;
            private ConsoleColor backColor;

            public ColorsMappingWrapper(LevelColors levelColors)
            {
                if (levelColors == null) return;
                var type = typeof(LevelColors);
                if ((bool)type.GetField("hasForeColor", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(levelColors))
                {
                    ForegroundColor = levelColors.ForeColor;
                }
                if ((bool)type.GetField("hasBackColor", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(levelColors))
                {
                    BackgroundColor = levelColors.BackColor;
                }

                Level = levelColors.Level;
            }
        }
    }
}
