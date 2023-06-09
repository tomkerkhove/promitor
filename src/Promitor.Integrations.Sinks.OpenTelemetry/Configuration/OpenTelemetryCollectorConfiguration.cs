using OpenTelemetry.Exporter;

namespace Promitor.Integrations.Sinks.OpenTelemetry.Configuration
{
    public class OpenTelemetryCollectorSinkConfiguration
    {
        public CollectorInfoConfiguration CollectorInfo { get; set; }

        public class CollectorInfoConfiguration
        {
            public OtlpExportProtocol Protocol { get; set; } = default(OtlpExportProtocol);
            public string CollectorUri { get; set; }
            //Todo: Headers

        }
    }
}
