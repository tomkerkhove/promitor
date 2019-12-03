using Promitor.Core.Configuration.Model.Telemetry.Interfaces;
using Serilog.Events;

namespace Promitor.Core.Configuration.Model.Telemetry.Sinks
{
    public class ContainerLogConfiguration : ISinkConfiguration
    {
        public LogEventLevel? Verbosity { get; set; } = Defaults.Telemetry.ContainerLogs.Verbosity;
        public bool IsEnabled { get; set; } = Defaults.Telemetry.ContainerLogs.IsEnabled;
    }
}
