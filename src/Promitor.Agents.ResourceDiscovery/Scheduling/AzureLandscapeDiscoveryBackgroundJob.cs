using System;
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
        private readonly ILogger<AzureLandscapeDiscoveryBackgroundJob> _logger;
        // TODO: Refactor this one
        private readonly IRuntimeMetricsCollector _runtimeMetricsCollector;
        private readonly IMetricFactory _metricFactory;
        private readonly IAzureResourceRepository _azureResourceRepository;

        public AzureLandscapeDiscoveryBackgroundJob(IAzureResourceRepository azureResourceRepository,  IMetricFactory metricFactory, ILogger<AzureLandscapeDiscoveryBackgroundJob> logger)
        {
            Guard.NotNull(metricFactory, nameof(metricFactory));
            Guard.NotNull(azureResourceRepository, nameof(azureResourceRepository));

            _logger = logger;
            _metricFactory = metricFactory;
            _azureResourceRepository = azureResourceRepository;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Discovering Azure Landscape!");

            Dictionary<string, string> labels = new Dictionary<string, string>
            {
                { "tenant_id", "ABC" },
                { "subscription_id", "ABC" },
                { "resource_group", "ABC" },
            };
            var orderedLabels = labels.OrderByDescending(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var gauge = _metricFactory.CreateGauge("name", help: "description", includeTimestamp: true, labelNames: orderedLabels.Keys.ToArray());
            gauge.WithLabels(orderedLabels.Values.ToArray()).Set(1);

            return Task.CompletedTask;
        }

        public string Name => "Azure Landscape Discovery";
    }
}
