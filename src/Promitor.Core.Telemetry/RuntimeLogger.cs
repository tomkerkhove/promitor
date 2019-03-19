using Microsoft.Extensions.Logging.Console;
#pragma warning disable 618

namespace Promitor.Core.Telemetry
{
    public class RuntimeLogger : ConsoleLogger
    {
        public RuntimeLogger() : base("Runtime", (loggerName, logLevel) => true, includeScopes: true)
        {
        }
    }
}