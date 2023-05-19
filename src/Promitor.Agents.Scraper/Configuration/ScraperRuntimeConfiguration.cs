using Promitor.Agents.Core.Configuration;
using Promitor.Agents.Scraper.Configuration.Sinks;
using Promitor.Core.Scraping.Configuration.Runtime;
using Promitor.Integrations.AzureMonitor.Configuration;

namespace Promitor.Agents.Scraper.Configuration
{
    public class ScraperRuntimeConfiguration : RuntimeConfiguration
    {
        public AzureMonitorConfiguration AzureMonitor { get; set; } = new();
        public MetricsConfiguration MetricsConfiguration { get; set; } = new();
        public MetricSinkConfiguration MetricSinks { get; set; } = new();
        public ResourceDiscoveryConfiguration ResourceDiscovery { get; set; }
    }
}