using OpenTelemetry.Exporter;

namespace Promitor.Integrations.Sinks.OpenTelemetry.Configuration
{
    public class OpenTelemetryCollectorSinkConfiguration
    {
        public string CollectorUri { get; set; }
        
        public CollectorInfo Collector { get; set; }

        public class CollectorInfo
        {
            public string CollectorProtocol { get; set; } = "grpc";
        }
    }
}
