using Serilog.Events;

namespace Promitor.Core.Configuration.Model.Telemetry.Interfaces
{
    public interface ISinkConfiguration
    {
        LogEventLevel? Verbosity { get; }
        bool IsEnabled { get; }
    }
}
