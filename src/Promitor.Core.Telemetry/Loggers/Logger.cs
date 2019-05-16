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