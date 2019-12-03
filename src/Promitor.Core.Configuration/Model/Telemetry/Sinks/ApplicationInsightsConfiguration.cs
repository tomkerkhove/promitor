using Promitor.Core.Configuration.Model.Telemetry.Interfaces;
using Serilog.Events;

namespace Promitor.Core.Configuration.Model.Telemetry.Sinks
{
    public class ApplicationInsightsConfiguration : ISinkConfiguration
    {
        public LogEventLevel? Verbosity { get; set; } = Defaults.Telemetry.ApplicationInsights.Verbosity;
        public bool IsEnabled { get; set; } = Defaults.Telemetry.ApplicationInsights.IsEnabled;
        public string InstrumentationKey { get; set; }
    }
}