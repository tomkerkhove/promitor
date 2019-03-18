using Microsoft.Extensions.Logging.Console;

namespace Promitor.Core.Telemetry
{
    public class RuntimeLogger : ConsoleLogger
    {
        public RuntimeLogger() : base("Runtime", (loggerName, logLevel) => true, includeScopes: true)
        {
        }
    }
}