using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Promitor.Core.Configuration.Telemetry;

#pragma warning disable 618

namespace Promitor.Core.Telemetry.Loggers
{
    public class Logger : ConsoleLogger
    {
        public Logger(string name, IConfiguration configuration) : base(name, (loggerName, logLevel) => IsFilteringRequired(logLevel, configuration), includeScopes: true)
        {
        }

        public override void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
        {
            message = $"[{DateTimeOffset.UtcNow:u}] {message}";
            base.WriteMessage(logLevel, logName, eventId, message, exception);
        }

        private static bool IsFilteringRequired(LogLevel usedLogLevel, IConfiguration configuration)
        {
            var telemetryConfiguration = configuration.GetSection("telemetry").Get<TelemetryConfiguration>();
            if (telemetryConfiguration?.ContainerLogs == null || telemetryConfiguration.ContainerLogs.IsEnabled == false)
            {
                return true;
            }

            LogLevel minimalLogLevel = telemetryConfiguration.DefaultVerbosity ?? telemetryConfiguration.ContainerLogs?.Verbosity ?? LogLevel.Warning;

            return minimalLogLevel <= usedLogLevel;
        }
    }
}