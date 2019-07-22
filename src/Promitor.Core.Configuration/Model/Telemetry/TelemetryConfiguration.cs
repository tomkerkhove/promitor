using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.Model.Telemetry.Sinks;

namespace Promitor.Core.Configuration.Model.Telemetry
{
    public class TelemetryConfiguration
    {
        public LogLevel? DefaultVerbosity { get; set; } = Defaults.Telemetry.DefaultVerbosity;
        public ContainerLogConfiguration ContainerLogs { get; set; } = new ContainerLogConfiguration();
        public ApplicationInsightsConfiguration ApplicationInsights { get; set; } = new ApplicationInsightsConfiguration();
    }
}
