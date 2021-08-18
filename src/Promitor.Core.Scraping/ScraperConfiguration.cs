using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Metrics.Prometheus.Collectors.Interfaces;
using Promitor.Core.Metrics.Sinks;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Core.Scraping
{
    public class ScraperConfiguration
    {
        /// <summary>
        ///     Client to interact with Azure Monitor
        /// </summary>
        public AzureMonitorClient AzureMonitorClient { get; }

        /// <summary>
        ///     Writer to send metrics to all configured sinks
        /// </summary>
        public MetricSinkWriter MetricSinkWriter { get; }

        /// <summary>
        ///     Logger used for telemetry
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        ///     Collector to send metrics related to the runtime
        /// </summary>
        public IAzureScrapingPrometheusMetricsCollector AzureScrapingPrometheusMetricsCollector { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="azureMonitorClient">Client to communicate with Azure Monitor</param>
        /// <param name="metricSinkWriter">Writer to send metrics to all configured sinks</param>
        /// <param name="azureScrapingPrometheusMetricsCollector">Collector to send metrics related to the runtime</param>
        /// <param name="logger">General logger</param>
        public ScraperConfiguration(AzureMonitorClient azureMonitorClient, MetricSinkWriter metricSinkWriter, IAzureScrapingPrometheusMetricsCollector azureScrapingPrometheusMetricsCollector, ILogger logger)
        {
            Guard.NotNull(azureMonitorClient, nameof(azureMonitorClient));
            Guard.NotNull(logger, nameof(logger));
            Guard.NotNull(metricSinkWriter, nameof(metricSinkWriter));
            Guard.NotNull(azureScrapingPrometheusMetricsCollector, nameof(azureScrapingPrometheusMetricsCollector));

            AzureMonitorClient = azureMonitorClient;
            Logger = logger;
            MetricSinkWriter = metricSinkWriter;
            AzureScrapingPrometheusMetricsCollector = azureScrapingPrometheusMetricsCollector;
        }
    }
}