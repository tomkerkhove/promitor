using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Configuration.Telemetry.Sinks;

namespace Promitor.Agents.Core.Configuration.Telemetry
{
    public class TelemetryConfiguration
    {
        public LogLevel? DefaultVerbosity { get; set; } = Defaults.Telemetry.DefaultVerbosity;
        public ContainerLogConfiguration ContainerLogs { get; set; } = new();
        public ApplicationInsightsConfiguration ApplicationInsights { get; set; } = new();
    }
}
