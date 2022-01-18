using Promitor.Integrations.Sinks.Atlassian.Statuspage.Configuration;
using Promitor.Integrations.Sinks.OpenTelemetry.Configuration;
using Promitor.Integrations.Sinks.Prometheus.Configuration;
using Promitor.Integrations.Sinks.Statsd.Configuration;

namespace Promitor.Agents.Scraper.Configuration.Sinks
{
    public class MetricSinkConfiguration
    {
        public AtlassianStatusPageSinkConfiguration AtlassianStatuspage { get; set; }
        public OpenTelemetryCollectorSinkConfiguration OpenTelemetryCollector { get; set; }
        public PrometheusScrapingEndpointSinkConfiguration PrometheusScrapingEndpoint { get; set; }
        public StatsdSinkConfiguration Statsd { get; set; }
    }
}