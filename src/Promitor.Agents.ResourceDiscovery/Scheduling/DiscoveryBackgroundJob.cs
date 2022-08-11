﻿using System.Collections.Generic;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces;
using Promitor.Core.Metrics.Prometheus.Collectors.Interfaces;

namespace Promitor.Agents.ResourceDiscovery.Scheduling
{
    public class DiscoveryBackgroundJob
    {
        protected IAzureResourceRepository AzureResourceRepository { get; }
        protected ILogger Logger { get; }
        
        private readonly ISystemMetricsCollector _systemMetricsCollector;

        public DiscoveryBackgroundJob(IAzureResourceRepository azureResourceRepository, ISystemMetricsCollector systemMetricsCollector, ILogger logger)
        {
            Guard.NotNull(systemMetricsCollector, nameof(systemMetricsCollector));
            Guard.NotNull(azureResourceRepository, nameof(azureResourceRepository));

            Logger = logger;
            _systemMetricsCollector = systemMetricsCollector;
            AzureResourceRepository = azureResourceRepository;
        }

        protected void WritePrometheusMetric(string metricName, string metricDescription, int value, Dictionary<string, string> labels)
        {
            _systemMetricsCollector.WriteGaugeMeasurement(metricName, metricDescription, value, labels, includeTimestamp: true);
        }

        protected string GetValueOrDefault(string preferredValue, string alternative)
        {
            if (string.IsNullOrWhiteSpace(preferredValue))
            {
                return alternative;
            }

            return preferredValue;
        }
    }
}
