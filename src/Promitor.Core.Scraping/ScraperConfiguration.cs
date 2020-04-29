using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Prometheus.Interfaces;
using Promitor.Core.Scraping.Sinks;
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
        ///     Sink to write all metrics to
        /// </summary>
        public IMetricSink Sink { get; }

        /// <summary>
        ///     Logger used for telemetry
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="azureMonitorClient">Client to communicate with Azure Monitor</param>
        /// <param name="sink">Sink to write all metrics to</param>
        /// <param name="prometheusMetricWriter">Metrics collector for our Prometheus scraping endpoint</param>
        /// <param name="logger">General logger</param>
        public ScraperConfiguration(AzureMonitorClient azureMonitorClient, IMetricSink sink, IPrometheusMetricWriter prometheusMetricWriter, ILogger logger)
        {
            Guard.NotNull(azureMonitorClient, nameof(azureMonitorClient));
            Guard.NotNull(prometheusMetricWriter, nameof(prometheusMetricWriter));
            Guard.NotNull(logger, nameof(logger));
            Guard.NotNull(sink, nameof(sink));

            AzureMonitorClient = azureMonitorClient;
            PrometheusMetricWriter = prometheusMetricWriter;
            Logger = logger;
            Sink = sink;
        }
    }
}