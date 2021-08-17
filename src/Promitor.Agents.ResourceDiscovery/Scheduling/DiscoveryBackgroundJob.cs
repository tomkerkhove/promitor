using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Microsoft.Extensions.Logging;
using Prometheus.Client;
using Promitor.Agents.ResourceDiscovery.Repositories.Interfaces;
using Promitor.Core.Metrics;

namespace Promitor.Agents.ResourceDiscovery.Scheduling
{
    public class DiscoveryBackgroundJob
    {
        private readonly IMetricFactory _metricFactory;
        protected IAzureResourceRepository AzureResourceRepository { get; }
        protected ILogger Logger { get; }

        // TODO: Refactor this one
        private readonly IRuntimeMetricsCollector _runtimeMetricsCollector;

        public DiscoveryBackgroundJob(IAzureResourceRepository azureResourceRepository, IMetricFactory metricFactory, ILogger logger)
        {
            Guard.NotNull(metricFactory, nameof(metricFactory));
            Guard.NotNull(azureResourceRepository, nameof(azureResourceRepository));

            Logger = logger;
            _metricFactory = metricFactory;
            AzureResourceRepository = azureResourceRepository;
        }

        protected void WritePrometheusMetric(string metricName, string metricDescription, int value, Dictionary<string, string> labels)
        {
            // Order labels alphabetically
            var orderedLabels = labels.OrderByDescending(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var gauge = _metricFactory.CreateGauge(metricName, help: metricDescription, includeTimestamp: true, labelNames: orderedLabels.Keys.ToArray());
            gauge.WithLabels(orderedLabels.Values.ToArray()).Set(value);
        }
    }
}
