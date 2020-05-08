using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Configuration.Telemetry.Interfaces;

namespace Promitor.Agents.Core.Configuration.Telemetry.Sinks
{
    public class ApplicationInsightsConfiguration : ISinkConfiguration
    {
        public LogLevel? Verbosity { get; set; } = Defaults.Telemetry.ApplicationInsights.Verbosity;
        public bool IsEnabled { get; set; } = Defaults.Telemetry.ApplicationInsights.IsEnabled;
        public string InstrumentationKey { get; set; }
    }
}