using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

#pragma warning disable 618

namespace Promitor.Core.Telemetry.Loggers
{
    public class Logger : ConsoleLogger
    {
        public Logger(string name) : base(name, (loggerName, logLevel) => IsFilteringRequired(logLevel), includeScopes: true)
        {
        }

        public override void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
        {
            message = $"[{DateTimeOffset.UtcNow.ToString("u")}] {message}";
            base.WriteMessage(logLevel, logName, eventId, message, exception);
        }

        private static bool IsFilteringRequired(LogLevel usedLogLevel)
        {
            var rawMinimalLogLevel = Environment.GetEnvironmentVariable(EnvironmentVariables.Logging.MinimumLogLevel);
            if (Enum.TryParse(rawMinimalLogLevel, out LogLevel minimalLogLevel))
            {
                return minimalLogLevel <= usedLogLevel;
            }

            return LogLevel.Warning <= usedLogLevel;
        }
    }
}