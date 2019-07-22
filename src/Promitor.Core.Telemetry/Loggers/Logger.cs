using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Promitor.Core.Configuration.Model.Telemetry;

#pragma warning disable 618

namespace Promitor.Core.Telemetry.Loggers
{
    public class Logger : ConsoleLogger
    {
        public Logger(string name, IOptionsMonitor<TelemetryConfiguration> configuration) : base(name, (loggerName, logLevel) => IsFilteringRequired(logLevel, configuration), includeScopes: true)
        {
        }

        public override void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
        {
            message = $"[{DateTimeOffset.UtcNow:u}] {message}";
            base.WriteMessage(logLevel, logName, eventId, message, exception);
        }

        private static bool IsFilteringRequired(LogLevel usedLogLevel, IOptionsMonitor<TelemetryConfiguration> configuration)
        {
            var telemetryConfiguration = configuration.CurrentValue;
            if (telemetryConfiguration?.ContainerLogs == null || telemetryConfiguration.ContainerLogs.IsEnabled == false)
            {
                return true;
            }

            LogLevel minimalLogLevel = telemetryConfiguration.ContainerLogs?.Verbosity ?? telemetryConfiguration.DefaultVerbosity ?? LogLevel.Warning;

            return minimalLogLevel <= usedLogLevel;
        }
    }
}