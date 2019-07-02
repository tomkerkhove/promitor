using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Core.Configuration.Telemetry;

#pragma warning disable 618

namespace Promitor.Core.Telemetry.Loggers
{
    public class RuntimeLogger : Logger
    {
        public RuntimeLogger(IOptionsMonitor<TelemetryConfiguration> configuration) : base("Runtime", configuration)
        {
        }
    }
}