using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.Extensions.Scheduler;
using GuardNet;
using Microsoft.Extensions.Logging;
using Prometheus.Client;
using Promitor.Agents.ResourceDiscovery.Repositories;
using Promitor.Agents.ResourceDiscovery.Repositories.Interfaces;
using Promitor.Core.Metrics;

namespace Promitor.Agents.ResourceDiscovery.Scheduling
{
    public class AzureSubscriptionDiscoveryBackgroundJob : IScheduledJob
    {
        public const string MetricName = "promitor_azure_landscape_subscription_info";
        public const string MetricDescription = "Provides information concerning the Azure subscriptions in the landscape that Promitor has access to.";
        private readonly ILogger<AzureSubscriptionDiscoveryBackgroundJob> _logger;

        // TODO: Refactor this one
        private readonly IRuntimeMetricsCollector _runtimeMetricsCollector;
        private readonly IMetricFactory _metricFactory;
        private readonly IAzureResourceRepository _azureResourceRepository;

        public AzureSubscriptionDiscoveryBackgroundJob(string jobName, IAzureResourceRepository azureResourceRepository,  IMetricFactory metricFactory, ILogger<AzureSubscriptionDiscoveryBackgroundJob> logger)
        {
            Guard.NotNull(metricFactory, nameof(metricFactory));
            Guard.NotNull(azureResourceRepository, nameof(azureResourceRepository));

            Name = jobName;
            _logger = logger;
            _metricFactory = metricFactory;
            _azureResourceRepository = azureResourceRepository;
        }

        public string Name { get; }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Discovering Azure Landscape!");

            // Discover Azure subscriptions
            var discoveredLandscape = await _azureResourceRepository.DiscoverAzureSubscriptionsAsync();

            // Report discovered information as metric
            foreach (var discoveredLandscapeItem in discoveredLandscape)
            {
                ReportDiscoveredAzureInfo(discoveredLandscapeItem);
            }
        }

        private void ReportDiscoveredAzureInfo(AzureSubscriptionInformation azureLandscapeInformation)
        {
            // Compose metric labels
            var labels = new Dictionary<string, string>
            {
                { "tenant_id", azureLandscapeInformation.TenantId },
                { "subscription_id", azureLandscapeInformation.Id },
                { "subscription_name", azureLandscapeInformation.Name},
                { "quota_id", azureLandscapeInformation.QuotaId},
                { "spending_limit", azureLandscapeInformation.SpendingLimit},
                { "state", azureLandscapeInformation.State},
                { "authorization", azureLandscapeInformation.AuthorizationSource}
            };

            // Order labels alphabetically
            var orderedLabels = labels.OrderByDescending(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // Report metric in Prometheus endpoint
            WritePrometheusMetric(value: 1, orderedLabels);
        }

        private void WritePrometheusMetric(int value , Dictionary<string, string> orderedLabels)
        {
            var gauge = _metricFactory.CreateGauge(MetricName, help: MetricDescription, includeTimestamp: true, labelNames: orderedLabels.Keys.ToArray());
            gauge.WithLabels(orderedLabels.Values.ToArray()).Set(value);
        }
    }
}
