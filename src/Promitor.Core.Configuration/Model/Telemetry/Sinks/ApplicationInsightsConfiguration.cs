using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.Model.Telemetry.Interfaces;

namespace Promitor.Core.Configuration.Model.Telemetry.Sinks
{
    public class ApplicationInsightsConfiguration : ISinkConfiguration
    {
        public LogLevel? Verbosity { get; set; } = Defaults.Telemetry.ApplicationInsights.Verbosity;
        public bool IsEnabled { get; set; } = Defaults.Telemetry.ApplicationInsights.IsEnabled;
        public string InstrumentationKey { get; set; }
    }
}