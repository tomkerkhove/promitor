using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.Telemetry.Sinks;

namespace Promitor.Core.Configuration.Telemetry
{
    public class TelemetryConfiguration
    {
        public LogLevel? DefaultVerbosity { get; set; }
        public ContainerLogConfiguration ContainerLogs { get; set; }
        public ApplicationInsightsConfiguration ApplicationInsights { get; set; }
    }
}
