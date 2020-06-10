using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace AWS.TwitchBot.AWS.Core
{
    public abstract class LoggingResource
    {
        private string _logName;
        protected ILogger _Logger { get; }
        protected LoggingResource(string logname)
        {
            _logName = logname;
            _Logger = Logger.GetLogger(_logName);
        }
    }


    public sealed class Logger
    {
        private static LoggerFactory Factory { get; }
        private Logger() { }
        static Logger()
        {
            Factory = new LoggerFactory();
            Configure(Factory);
        }

        private static void Configure(LoggerFactory factory)
        {
#if DEBUG
            var currentLevel = LogLevel.Trace;
#else
            var currentLevel = LogLevel.Debug;
#endif
            if (Environment.GetEnvironmentVariable("LOGGING_LEVEL").IsInteger(out var logLevel))
            {
                currentLevel = (LogLevel)logLevel;
            }
            factory.AddLambdaLogger(new LambdaLoggerOptions
            {
                Filter = (s, level) => level >= currentLevel
            });
        }

        public static ILogger GetLogger(string category) => Factory.CreateLogger($"{category} - {Assembly.GetExecutingAssembly().GetName().Version}");
    }
}
