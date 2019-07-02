using Microsoft.Extensions.Options;
using Promitor.Core.Configuration.Model.Telemetry;

#pragma warning disable 618

namespace Promitor.Core.Telemetry.Loggers
{
    public class ValidationLogger : Logger
    {
        public ValidationLogger(IOptionsMonitor<TelemetryConfiguration> configuration) : base("Validation", configuration)
        {
        }
    }
}