using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

#pragma warning disable 618

namespace Promitor.Core.Telemetry.Loggers
{
    public class RuntimeLogger : Logger
    {
        public RuntimeLogger(IConfiguration configuration) : base("Runtime", configuration)
        {
        }
    }
}