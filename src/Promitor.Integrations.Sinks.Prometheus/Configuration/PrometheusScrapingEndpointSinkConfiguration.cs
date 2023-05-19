namespace Promitor.Integrations.Sinks.Prometheus.Configuration
{
    public class PrometheusScrapingEndpointSinkConfiguration
    {
        public string BaseUriPath { get; set; } = Defaults.Prometheus.ScrapeEndpointBaseUri;
        public double? MetricUnavailableValue { get; set; } = Defaults.Prometheus.MetricUnavailableValue;
        public bool EnableMetricTimestamps { get; set; } = Defaults.Prometheus.EnableMetricTimestamps;
        public LabelConfiguration Labels { get; set; } = new();
    }
}
