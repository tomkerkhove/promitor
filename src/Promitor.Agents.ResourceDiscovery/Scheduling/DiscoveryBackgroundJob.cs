using System.Collections.Generic;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces;
using Promitor.Core.Metrics.Prometheus.Collectors.Interfaces;

namespace Promitor.Agents.ResourceDiscovery.Scheduling
{
    public class DiscoveryBackgroundJob
    {
        protected IAzureResourceRepository AzureResourceRepository { get; }
        protected ILogger Logger { get; }
        
        private readonly IPrometheusMetricsCollector _prometheusMetricsCollector;

        public DiscoveryBackgroundJob(IAzureResourceRepository azureResourceRepository, IPrometheusMetricsCollector prometheusMetricsCollector, ILogger logger)
        {
            Guard.NotNull(prometheusMetricsCollector, nameof(prometheusMetricsCollector));
            Guard.NotNull(azureResourceRepository, nameof(azureResourceRepository));

            Logger = logger;
            _prometheusMetricsCollector = prometheusMetricsCollector;
            AzureResourceRepository = azureResourceRepository;
        }

        protected void WritePrometheusMetric(string metricName, string metricDescription, int value, Dictionary<string, string> labels)
        {
            _prometheusMetricsCollector.WriteGaugeMeasurement(metricName, metricDescription, value, labels, includeTimestamp: true);
        }
    }
}
