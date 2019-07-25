namespace Promitor.Core.Configuration.Model.Prometheus
{
    public class ScrapeEndpointConfiguration
    {
        public string BaseUriPath { get; set; } = Defaults.Prometheus.ScrapeEndpointBaseUri;
    }
}