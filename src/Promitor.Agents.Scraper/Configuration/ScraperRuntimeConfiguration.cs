﻿using Promitor.Agents.Core.Configuration;
using Promitor.Agents.Scraper.Configuration.Sinks;
using Promitor.Core.Scraping.Configuration.Runtime;
using Promitor.Integrations.AzureMonitor.Configuration;
using PrometheusSinkConfiguration = Promitor.Integrations.Sinks.Prometheus.Configuration.PrometheusSinkConfiguration;

namespace Promitor.Agents.Scraper.Configuration
{
    public class ScraperRuntimeConfiguration : RuntimeConfiguration
    {
        public PrometheusSinkConfiguration PrometheusSink { get; set; } = new PrometheusSinkConfiguration();
        public MetricSinkConfiguration MetricSinks { get; set; } = new MetricSinkConfiguration();
        public MetricsConfiguration MetricsConfiguration { get; set; } = new MetricsConfiguration();
        public AzureMonitorConfiguration AzureMonitor { get; set; } = new AzureMonitorConfiguration();
    }
}