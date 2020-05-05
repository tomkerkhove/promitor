namespace Promitor.Integrations.Sinks.Prometheus.Configuration
{
    // TODO: Move to Promitor.Integrations.Sinks.Prometheus
    public class PrometheusConfiguration
    {
        public ScrapeEndpointConfiguration ScrapeEndpoint { get; set; } = new ScrapeEndpointConfiguration();
        public double? MetricUnavailableValue { get; set; } = Defaults.Prometheus.MetricUnavailableValue;
        public bool EnableMetricTimestamps { get; set; } = Defaults.Prometheus.EnableMetricTimestamps;
    }
}
