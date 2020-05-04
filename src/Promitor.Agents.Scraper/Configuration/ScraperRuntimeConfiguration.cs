using Promitor.Agents.Core.Configuration;
using Promitor.Agents.Scraper.Configuration.Sinks;
using Promitor.Core.Scraping.Configuration.Runtime;
using Promitor.Core.Telemetry.Metrics.Configuration;
using Promitor.Integrations.AzureMonitor.Configuration;

namespace Promitor.Agents.Scraper.Configuration
{
    public class ScraperRuntimeConfiguration: RuntimeConfiguration2
    {
        public PrometheusConfiguration Prometheus { get; set; } = new PrometheusConfiguration();
        public MetricSinkConfiguration MetricSinks { get; set; } = new MetricSinkConfiguration();
        public MetricsConfiguration MetricsConfiguration { get; set; } = new MetricsConfiguration();
        public AzureMonitorConfiguration AzureMonitor { get; set; } = new AzureMonitorConfiguration();
    }
}