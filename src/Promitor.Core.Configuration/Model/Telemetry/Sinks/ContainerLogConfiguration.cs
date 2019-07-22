using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.Model.Telemetry.Interfaces;

namespace Promitor.Core.Configuration.Model.Telemetry.Sinks
{
    public class ContainerLogConfiguration : ISinkConfiguration
    {
        public LogLevel? Verbosity { get; set; } = Defaults.Telemetry.ContainerLogs.Verbosity;
        public bool IsEnabled { get; set; } = Defaults.Telemetry.ContainerLogs.IsEnabled;
    }
}
