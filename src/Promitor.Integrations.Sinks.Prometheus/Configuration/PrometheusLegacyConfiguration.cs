namespace Promitor.Integrations.Sinks.Prometheus.Configuration
{
    public class PrometheusLegacyConfiguration
    {
        public ScrapeEndpointConfiguration ScrapeEndpoint { get; set; } = new ScrapeEndpointConfiguration();
        public double? MetricUnavailableValue { get; set; } = Defaults.Prometheus.MetricUnavailableValue;
        public bool EnableMetricTimestamps { get; set; } = Defaults.Prometheus.EnableMetricTimestamps;
    }
}
