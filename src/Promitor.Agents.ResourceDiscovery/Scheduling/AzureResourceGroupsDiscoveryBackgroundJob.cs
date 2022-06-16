using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.Extensions.Scheduler;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces;
using Promitor.Core.Contracts;
using Promitor.Core.Metrics.Prometheus.Collectors.Interfaces;

namespace Promitor.Agents.ResourceDiscovery.Scheduling
{
    public class AzureResourceGroupsDiscoveryBackgroundJob : DiscoveryBackgroundJob, IScheduledJob
    {
        public const string MetricName = "promitor_azure_landscape_resource_group_info";
        public const string MetricDescription = "Provides information concerning the Azure resource groups in the landscape that Promitor has access to.";
        
        public AzureResourceGroupsDiscoveryBackgroundJob(string jobName, IAzureResourceRepository azureResourceRepository, IPrometheusMetricsCollector prometheusMetricsCollector, ILogger<AzureResourceGroupsDiscoveryBackgroundJob> logger)
            : base(azureResourceRepository, prometheusMetricsCollector, logger)
        {
            Guard.NotNullOrWhitespace(jobName, nameof(jobName));

            Name = jobName;
        }

        public string Name { get; }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogTrace("Discovering Azure Resource Groups...");

            PagedPayload<AzureResourceGroupInformation> discoveredResourceGroups;
            do
            {
                // Discover Azure subscriptions
                discoveredResourceGroups = await AzureResourceRepository.DiscoverAzureResourceGroupsAsync(pageSize: 1000, currentPage: 0);

                // Report discovered information as metric
                foreach (var resourceGroupInformation in discoveredResourceGroups.Result)
                {
                    ReportDiscoveredAzureInfo(resourceGroupInformation);
                }
            }
            while (discoveredResourceGroups.HasMore);

            Logger.LogTrace("Azure Resource Groups discovered.");
        }

        private void ReportDiscoveredAzureInfo(AzureResourceGroupInformation resourceGroupInformation)
        {
            var labels = new Dictionary<string, string>
            {
                { "tenant_id", resourceGroupInformation.TenantId },
                { "subscription_id", resourceGroupInformation.SubscriptionId },
                { "resource_group_name", resourceGroupInformation.Name },
                { "provisioning_state", GetValueOrDefault(resourceGroupInformation.ProvisioningState, "n/a") },
                { "managed_by", GetValueOrDefault(resourceGroupInformation.ManagedBy, "n/a") },
                { "region", GetValueOrDefault(resourceGroupInformation.Region, "n/a") }
            };

            // Report metric in Prometheus endpoint
            WritePrometheusMetric(MetricName, MetricDescription, value: 1, labels);
        }
    }
}
