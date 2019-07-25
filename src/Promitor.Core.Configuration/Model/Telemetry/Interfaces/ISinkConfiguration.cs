using Microsoft.Extensions.Logging;

namespace Promitor.Core.Configuration.Model.Telemetry.Interfaces
{
    public interface ISinkConfiguration
    {
        LogLevel? Verbosity { get; }
        bool IsEnabled { get; }
    }
}
