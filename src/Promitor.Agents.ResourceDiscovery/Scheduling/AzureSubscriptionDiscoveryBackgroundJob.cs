using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.Extensions.Scheduler;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces;
using Promitor.Core.Contracts;
using Promitor.Core.Metrics.Interfaces;

namespace Promitor.Agents.ResourceDiscovery.Scheduling
{
    public class AzureSubscriptionDiscoveryBackgroundJob : DiscoveryBackgroundJob, IScheduledJob
    {
        public const string MetricName = "promitor_azure_landscape_subscription_info";
        public const string MetricDescription = "Provides information concerning the Azure subscriptions in the landscape that Promitor has access to.";
        
        public AzureSubscriptionDiscoveryBackgroundJob(string jobName, IAzureResourceRepository azureResourceRepository, ISystemMetricsPublisher systemMetricsPublisher, ILogger<AzureSubscriptionDiscoveryBackgroundJob> logger)
            : base(azureResourceRepository, systemMetricsPublisher, logger)
        {
            Guard.NotNullOrWhitespace(jobName, nameof(jobName));

            Name = jobName;
        }

        public string Name { get; }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogTrace("Discovering Azure subscriptions...");

            // Discover Azure subscriptions

            PagedPayload<AzureSubscriptionInformation> discoveredLandscape;
            var currentPage = 1;
            do
            {
                discoveredLandscape = await AzureResourceRepository.DiscoverAzureSubscriptionsAsync(pageSize: 1000, currentPage: currentPage);

                // Report discovered information as metric
                foreach (var discoveredLandscapeItem in discoveredLandscape.Result)
                {
                    ReportDiscoveredAzureInfo(discoveredLandscapeItem);
                }

                currentPage++;
            }
            while (discoveredLandscape.HasMore);

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
                { "quota_id", GetValueOrDefault(azureLandscapeInformation.QuotaId, "n/a")},
                { "spending_limit", GetValueOrDefault(azureLandscapeInformation.SpendingLimit, "n/a")},
                { "state", GetValueOrDefault(azureLandscapeInformation.State, "n/a")},
                { "authorization", GetValueOrDefault(azureLandscapeInformation.AuthorizationSource, "n/a")}
            };

            // Report metric in Prometheus endpoint
            WritePrometheusMetricAsync(MetricName, MetricDescription, value: 1, labels).Wait();
        }
    }
}
