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
    public class AzureResourceGroupsDiscoveryBackgroundJob : IScheduledJob
    {
        public const string MetricName = "promitor_azure_landscape_resource_group_info";
        public const string MetricDescription = "Provides information concerning the Azure resource groups in the landscape that Promitor has access to.";
        private readonly ILogger<AzureResourceGroupsDiscoveryBackgroundJob> _logger;

        // TODO: Refactor this one
        private readonly IRuntimeMetricsCollector _runtimeMetricsCollector;
        private readonly IMetricFactory _metricFactory;
        private readonly IAzureResourceRepository _azureResourceRepository;

        public AzureResourceGroupsDiscoveryBackgroundJob(string jobName, IAzureResourceRepository azureResourceRepository,  IMetricFactory metricFactory, ILogger<AzureResourceGroupsDiscoveryBackgroundJob> logger)
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
            var discoveredResourceGroups = await _azureResourceRepository.DiscoverAzureResourceGroupsAsync();

            // Report discovered information as metric
            foreach (var resourceGroupInformation in discoveredResourceGroups)
            {
                ReportDiscoveredAzureInfo(resourceGroupInformation);
            }
        }

        private void ReportDiscoveredAzureInfo(AzureResourceGroupInformation resourceGroupInformation)
        {
            var managedByLabel = string.IsNullOrWhiteSpace(resourceGroupInformation.ManagedBy) ? "n/a" : resourceGroupInformation.ManagedBy;
            var labels = new Dictionary<string, string>
            {
                { "tenant_id", resourceGroupInformation.TenantId },
                { "subscription_id", resourceGroupInformation.SubscriptionId },
                { "resource_group_name", resourceGroupInformation.Name },
                { "provisioning_state", resourceGroupInformation.ProvisioningState },
                { "managed_by", managedByLabel },
                { "region", resourceGroupInformation.Region }
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
