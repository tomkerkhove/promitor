using Microsoft.Extensions.Logging;

namespace Promitor.Agents.Core.Configuration.Telemetry.Interfaces
{
    public interface ISinkConfiguration
    {
        LogLevel? Verbosity { get; }
        bool IsEnabled { get; }
    }
}
