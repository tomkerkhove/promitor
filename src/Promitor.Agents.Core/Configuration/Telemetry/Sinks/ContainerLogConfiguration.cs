using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Configuration.Telemetry.Interfaces;

namespace Promitor.Agents.Core.Configuration.Telemetry.Sinks
{
    public class ContainerLogConfiguration : ISinkConfiguration
    {
        public LogLevel? Verbosity { get; set; } = Defaults.Telemetry.ContainerLogs.Verbosity;
        public bool IsEnabled { get; set; } = Defaults.Telemetry.ContainerLogs.IsEnabled;
    }
}
