namespace Promitor.Core.Configuration.Model.Prometheus
{
    public class PrometheusConfiguration
    {
        public ScrapeEndpointConfiguration ScrapeEndpoint { get; set; } = new ScrapeEndpointConfiguration();
        public double? MetricUnavailableValue { get; set; } = Defaults.Prometheus.MetricUnavailableValue;
        public bool EnableMetricTimestamps { get; set; } = Defaults.Prometheus.EnableMetricTimestamps;
    }
}
