namespace Promitor.Core.Configuration.Model.Prometheus
{
    public class PrometheusConfiguration
    {
        public ScrapeEndpointConfiguration ScrapeEndpoint { get; set; } = new ScrapeEndpointConfiguration();
    }
}
