namespace Promitor.Core.Telemetry.Metrics.Configuration
{
    public class ScrapeEndpointConfiguration
    {
        public string BaseUriPath { get; set; } = Defaults.Prometheus.ScrapeEndpointBaseUri;
    }
}