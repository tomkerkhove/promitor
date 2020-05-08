namespace Promitor.Integrations.Sinks.Prometheus.Configuration
{
    public class ScrapeEndpointConfiguration
    {
        public string BaseUriPath { get; set; } = Defaults.Prometheus.ScrapeEndpointBaseUri;
    }
}