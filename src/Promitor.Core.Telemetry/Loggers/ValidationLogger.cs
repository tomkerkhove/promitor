using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

#pragma warning disable 618

namespace Promitor.Core.Telemetry.Loggers
{
    public class ValidationLogger : Logger
    {
        public ValidationLogger(IConfiguration configuration) : base("Validation", configuration)
        {
        }
    }
}