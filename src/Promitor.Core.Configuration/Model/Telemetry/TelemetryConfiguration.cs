using Promitor.Core.Configuration.Model.Telemetry.Sinks;
using Serilog.Events;

namespace Promitor.Core.Configuration.Model.Telemetry
{
    public class TelemetryConfiguration
    {
        public LogEventLevel? DefaultVerbosity { get; set; } = Defaults.Telemetry.DefaultVerbosity;
        public ContainerLogConfiguration ContainerLogs { get; set; } = new ContainerLogConfiguration();
        public ApplicationInsightsConfiguration ApplicationInsights { get; set; } = new ApplicationInsightsConfiguration();
    }
}
