using OpenTelemetry.Exporter;

namespace Promitor.Integrations.Sinks.OpenTelemetry.Configuration
{
    public class OpenTelemetryCollectorSinkConfiguration
    {
        public OtlpExportProtocol Protocol { get; set; } = default(OtlpExportProtocol);
        public string CollectorUri { get; set; }
        //Todo: Headers
    }
}
