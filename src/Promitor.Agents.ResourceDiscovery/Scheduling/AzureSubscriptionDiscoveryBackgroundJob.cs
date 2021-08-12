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
    public class DiscoveryBackgroundJob
    {
        private readonly IMetricFactory _metricFactory;
        protected IAzureResourceRepository AzureResourceRepository { get; }
        protected ILogger Logger { get; }

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
    public class AzureSubscriptionDiscoveryBackgroundJob : DiscoveryBackgroundJob, IScheduledJob
    {
        public const string MetricName = "promitor_azure_landscape_subscription_info";
        public const string MetricDescription = "Provides information concerning the Azure subscriptions in the landscape that Promitor has access to.";
        
        // TODO: Refactor this one
        private readonly IRuntimeMetricsCollector _runtimeMetricsCollector;

        public AzureSubscriptionDiscoveryBackgroundJob(string jobName, IAzureResourceRepository azureResourceRepository,  IMetricFactory metricFactory, ILogger<AzureSubscriptionDiscoveryBackgroundJob> logger)
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
            var discoveredLandscape = await AzureResourceRepository.DiscoverAzureSubscriptionsAsync();

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

            // Report metric in Prometheus endpoint
            WritePrometheusMetric(MetricName, MetricDescription, value: 1, labels);
        }
    }
}
