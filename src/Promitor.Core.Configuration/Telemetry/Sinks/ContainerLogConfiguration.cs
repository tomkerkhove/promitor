using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.Telemetry.Interfaces;

namespace Promitor.Core.Configuration.Telemetry.Sinks
{
    public class ContainerLogConfiguration : ISinkConfiguration
    {
        public LogLevel? Verbosity { get; set; }
        public bool IsEnabled { get; set; } = false;
    }
}
