using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.Extensions.Scheduler;
using GuardNet;
using Microsoft.Extensions.Logging;
using Prometheus.Client;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Agents.ResourceDiscovery.Repositories;
using Promitor.Agents.ResourceDiscovery.Repositories.Interfaces;
using Promitor.Core.Metrics;

namespace Promitor.Agents.ResourceDiscovery.Scheduling
{
    public class AzureResourceGroupsDiscoveryBackgroundJob : DiscoveryBackgroundJob, IScheduledJob
    {
        public const string MetricName = "promitor_azure_landscape_resource_group_info";
        public const string MetricDescription = "Provides information concerning the Azure resource groups in the landscape that Promitor has access to.";
        
        // TODO: Refactor this one
        private readonly IRuntimeMetricsCollector _runtimeMetricsCollector;

        public AzureResourceGroupsDiscoveryBackgroundJob(string jobName, IAzureResourceRepository azureResourceRepository,  IMetricFactory metricFactory, ILogger<AzureResourceGroupsDiscoveryBackgroundJob> logger)
            : base(azureResourceRepository, metricFactory, logger)
        {
            Guard.NotNullOrWhitespace(jobName, nameof(jobName));

            Name = jobName;
        }

        public string Name { get; }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Discovering Azure Landscape!");

            // Discover Azure subscriptions
            var discoveredResourceGroups = await AzureResourceRepository.DiscoverAzureResourceGroupsAsync();

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

            // Report metric in Prometheus endpoint
            WritePrometheusMetric(MetricName, MetricDescription, value: 1, labels);
        }
    }
}
