using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Metrics.Interfaces;
using Promitor.Core.Metrics.Sinks;
using Promitor.Integrations.AzureMonitor;
using Promitor.Integrations.LogAnalytics;

namespace Promitor.Core.Scraping
{
    public class ScraperConfiguration
    {
        /// <summary>
        ///     Client to interact with Azure Monitor
        /// </summary>
        public AzureMonitorClient AzureMonitorClient { get; }

        /// <summary>
        ///     Client to interact with Log Analytics
        /// </summary>
        public LogAnalyticsClient LogAnalyticsClient { get; }

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
        public IAzureScrapingSystemMetricsPublisher AzureScrapingSystemMetricsPublisher { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="azureMonitorClient">Client to communicate with Azure Monitor</param>
        /// <param name="logAnalyticsClient">Client to communicate with Log Analytics</param>
        /// <param name="metricSinkWriter">Writer to send metrics to all configured sinks</param>
        /// <param name="azureScrapingSystemMetricsPublisher">Collector to send metrics related to the runtime</param>
        /// <param name="logger">General logger</param>
        public ScraperConfiguration(AzureMonitorClient azureMonitorClient, LogAnalyticsClient logAnalyticsClient, MetricSinkWriter metricSinkWriter, IAzureScrapingSystemMetricsPublisher azureScrapingSystemMetricsPublisher, ILogger logger)
        {
            Guard.NotNull(azureMonitorClient, nameof(azureMonitorClient));
            Guard.NotNull(logger, nameof(logger));
            Guard.NotNull(metricSinkWriter, nameof(metricSinkWriter));
            Guard.NotNull(azureScrapingSystemMetricsPublisher, nameof(azureScrapingSystemMetricsPublisher));

            AzureMonitorClient = azureMonitorClient;
            LogAnalyticsClient = logAnalyticsClient;
            Logger = logger;
            MetricSinkWriter = metricSinkWriter;
            AzureScrapingSystemMetricsPublisher = azureScrapingSystemMetricsPublisher;
        }
    }
}