using Promitor.Agents.Core.Configuration;
using Promitor.Agents.Scraper.Configuration.Sinks;
using Promitor.Core.Scraping.Configuration.Runtime;
using Promitor.Integrations.AzureMonitor.Configuration;
using Promitor.Integrations.Sinks.Prometheus.Configuration;

namespace Promitor.Agents.Scraper.Configuration
{
    public class ScraperRuntimeConfiguration : RuntimeConfiguration
    {
        public AzureMonitorConfiguration AzureMonitor { get; set; } = new AzureMonitorConfiguration();
        public MetricsConfiguration MetricsConfiguration { get; set; } = new MetricsConfiguration();
        public MetricSinkConfiguration MetricSinks { get; set; } = new MetricSinkConfiguration();
        public PrometheusLegacyConfiguration Prometheus { get; set; }
        public ResourceDiscoveryConfiguration ResourceDiscovery { get; set; }
    }
}