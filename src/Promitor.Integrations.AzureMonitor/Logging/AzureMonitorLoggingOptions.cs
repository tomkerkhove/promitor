using System;
using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.Model;

namespace Promitor.Integrations.AzureMonitor.Logging
{
    public class AzureMonitorLoggingOptions
    {
        public AzureMonitorLoggingOptions(IConfiguration configuration)
        {
            Guard.NotNull(configuration, nameof(configuration));

            var telemetryConfiguration = configuration.Get<RuntimeConfiguration>()?.Telemetry;
            if (telemetryConfiguration == null)
            {
                throw new Exception("Unable to get telemetry configuration");
            }

            var defaultVerbosity = telemetryConfiguration.DefaultVerbosity;
            var consoleVerbosity = telemetryConfiguration.ContainerLogs.Verbosity;

            if (defaultVerbosity != null)
            {
                IsEnabled = defaultVerbosity <= LogLevel.Debug;
            }

            if (consoleVerbosity != null)
            {
                IsEnabled = consoleVerbosity <= LogLevel.Debug;
            }
        }

        public bool IsEnabled { get; } = false;
    }
}