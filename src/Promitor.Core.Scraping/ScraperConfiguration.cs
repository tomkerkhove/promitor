using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Interfaces;
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
        ///     Metrics collector for our Prometheus scraping endpoint
        /// </summary>
        public IPrometheusMetricWriter PrometheusMetricWriter { get; }

        /// <summary>
        ///     Writer to send metrics to all configured sinks
        /// </summary>
        public MetricSinkWriter MetricSinkWriter { get; }

        /// <summary>
        ///     Logger used for telemetry
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="azureMonitorClient">Client to communicate with Azure Monitor</param>
        /// <param name="metricSinkWriter">Writer to send metrics to all configured sinks</param>
        /// <param name="prometheusMetricWriter">Metrics collector for our Prometheus scraping endpoint</param>
        /// <param name="logger">General logger</param>
        public ScraperConfiguration(AzureMonitorClient azureMonitorClient, MetricSinkWriter metricSinkWriter, IPrometheusMetricWriter prometheusMetricWriter, ILogger logger)
        {
            Guard.NotNull(azureMonitorClient, nameof(azureMonitorClient));
            Guard.NotNull(prometheusMetricWriter, nameof(prometheusMetricWriter));
            Guard.NotNull(logger, nameof(logger));
            Guard.NotNull(metricSinkWriter, nameof(metricSinkWriter));

            AzureMonitorClient = azureMonitorClient;
            PrometheusMetricWriter = prometheusMetricWriter;
            Logger = logger;
            MetricSinkWriter = metricSinkWriter;
        }
    }
}