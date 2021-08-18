using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.Extensions.Scheduler;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces;
using Promitor.Core.Metrics.Prometheus.Collectors.Interfaces;

namespace Promitor.Agents.ResourceDiscovery.Scheduling
{
    public class AzureSubscriptionDiscoveryBackgroundJob : DiscoveryBackgroundJob, IScheduledJob
    {
        public const string MetricName = "promitor_azure_landscape_subscription_info";
        public const string MetricDescription = "Provides information concerning the Azure subscriptions in the landscape that Promitor has access to.";
        
        public AzureSubscriptionDiscoveryBackgroundJob(string jobName, IAzureResourceRepository azureResourceRepository, IPrometheusMetricsCollector prometheusMetricsCollector, ILogger<AzureSubscriptionDiscoveryBackgroundJob> logger)
            : base(azureResourceRepository, prometheusMetricsCollector, logger)
        {
            Guard.NotNullOrWhitespace(jobName, nameof(jobName));

            Name = jobName;
        }

        public string Name { get; }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogTrace("Discovering Azure subscriptions...");

            // Discover Azure subscriptions
            var discoveredLandscape = await AzureResourceRepository.DiscoverAzureSubscriptionsAsync();

            // Report discovered information as metric
            foreach (var discoveredLandscapeItem in discoveredLandscape)
            {
                ReportDiscoveredAzureInfo(discoveredLandscapeItem);
            }

            Logger.LogTrace("Azure subscriptions discovered...");
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

            // Report metric in Prometheus endpoint
            WritePrometheusMetric(MetricName, MetricDescription, value: 1, labels);
        }
    }
}
