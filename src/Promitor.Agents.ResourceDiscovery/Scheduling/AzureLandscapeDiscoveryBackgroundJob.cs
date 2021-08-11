using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.Extensions.Scheduler;
using GuardNet;
using Microsoft.Extensions.Logging;
using Prometheus.Client;
using Promitor.Agents.ResourceDiscovery.Repositories.Interfaces;
using Promitor.Core.Metrics;

namespace Promitor.Agents.ResourceDiscovery.Scheduling
{
    public class AzureLandscapeDiscoveryBackgroundJob : IScheduledJob
    {
        public const string MetricName = "promitor_azure_landscape_info";
        public const string MetricDescription = "Provides information concerning the Azure landscape that Promitor has access to.";
        private readonly ILogger<AzureLandscapeDiscoveryBackgroundJob> _logger;

        // TODO: Refactor this one
        private readonly IRuntimeMetricsCollector _runtimeMetricsCollector;
        private readonly IMetricFactory _metricFactory;
        private readonly IAzureResourceRepository _azureResourceRepository;

        public AzureLandscapeDiscoveryBackgroundJob(string jobName, IAzureResourceRepository azureResourceRepository,  IMetricFactory metricFactory, ILogger<AzureLandscapeDiscoveryBackgroundJob> logger)
        {
            Guard.NotNull(metricFactory, nameof(metricFactory));
            Guard.NotNull(azureResourceRepository, nameof(azureResourceRepository));

            Name = jobName;
            _logger = logger;
            _metricFactory = metricFactory;
            _azureResourceRepository = azureResourceRepository;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Discovering Azure Landscape!");

            // TODO: Discover!

            // Report discovered information as metric
            ReportDiscoveredAzureInfo();

            return Task.CompletedTask;
        }

        private void ReportDiscoveredAzureInfo()
        {
            // TODO: Report metric per subscription, resource group
            var labels = new Dictionary<string, string>
            {
                { "tenant_id", "ABC" },
                { "subscription_id", "ABC" },
                { "resource_group", "ABC" },
            };
            var orderedLabels = labels.OrderByDescending(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            WritePrometheusMetric(orderedLabels);
        }

        private void WritePrometheusMetric(Dictionary<string, string> orderedLabels)
        {
            var gauge = _metricFactory.CreateGauge(MetricName, help: MetricDescription, includeTimestamp: true, labelNames: orderedLabels.Keys.ToArray());
            gauge.WithLabels(orderedLabels.Values.ToArray()).Set(1);
        }

        public string Name {get; }
    }
}
