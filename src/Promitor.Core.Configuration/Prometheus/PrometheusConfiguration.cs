namespace Promitor.Core.Configuration.Prometheus
{
    public class PrometheusConfiguration
    {
        public ScrapeEndpointConfiguration ScrapeEndpoint { get; set; } = new ScrapeEndpointConfiguration();
    }
}
