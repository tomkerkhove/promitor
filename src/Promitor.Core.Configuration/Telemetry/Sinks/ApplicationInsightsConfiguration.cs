using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.Telemetry.Interfaces;

namespace Promitor.Core.Configuration.Telemetry.Sinks
{
    public class ApplicationInsightsConfiguration : ISinkConfiguration
    {
        public LogLevel? Verbosity { get; set; }
        public bool IsEnabled { get; set; } = false;
        public string InstrumentationKey { get; set; }
    }
}