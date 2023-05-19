﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Options;
using Promitor.Core.Metrics.Interfaces;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Integrations.Sinks.Prometheus.Configuration;

namespace Promitor.Integrations.Sinks.Prometheus.Collectors
{
    public class AzureScrapingSystemMetricsPublisher : IAzureScrapingSystemMetricsPublisher
    {
        private readonly ISystemMetricsPublisher _systemMetricsPublisher;
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        private readonly IOptionsMonitor<PrometheusScrapingEndpointSinkConfiguration> _prometheusConfiguration;
        private readonly ConcurrentDictionary<string, string> _subscriptionMappings = new ConcurrentDictionary<string, string>();

        public AzureScrapingSystemMetricsPublisher(IMetricsDeclarationProvider metricsDeclarationProvider, ISystemMetricsPublisher systemMetricsPublisher, IOptionsMonitor<PrometheusScrapingEndpointSinkConfiguration> prometheusConfiguration)
        {
            Guard.NotNull(metricsDeclarationProvider, nameof(metricsDeclarationProvider));
            Guard.NotNull(systemMetricsPublisher, nameof(systemMetricsPublisher));

            _prometheusConfiguration = prometheusConfiguration;
            _systemMetricsPublisher = systemMetricsPublisher;
            _metricsDeclarationProvider = metricsDeclarationProvider;
        }

        /// <summary>
        /// Registers or updates a subscription ID to name mapping
        /// </summary>
        /// <param name="id">Subscription ID.</param>
        /// <param name="name">Subscription name.</param>
        public void RegisterSubscriptionMapping(string id, string name)
        {
            _subscriptionMappings[id] = name;
        }

        /// <summary>
        /// Sets a new value for a measurement on a gauge
        /// </summary>
        /// <param name="name">Name of the metric</param>
        /// <param name="description">Description of the metric</param>
        /// <param name="value">New measured value</param>
        /// <param name="labels">Labels that are applicable for this measurement</param>
        public async Task WriteGaugeMeasurementAsync(string name, string description, double value, Dictionary<string, string> labels)
        {
            var enableMetricTimestamps = _prometheusConfiguration.CurrentValue.EnableMetricTimestamps;

            var metricsDeclaration = _metricsDeclarationProvider.Get(applyDefaults: true);
            if (labels.ContainsKey("tenant_id") == false)
            {
                labels.Add("tenant_id", metricsDeclaration.AzureMetadata.TenantId);
            }

            addSubscriptionNameLabel(labels);

            var orderedLabels = labels.OrderByDescending(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            await _systemMetricsPublisher.WriteGaugeMeasurementAsync(name, description, value, orderedLabels, enableMetricTimestamps);
        }

        public async Task WriteGaugeMeasurementAsync(string name, string description, double value, Dictionary<string, string> labels, bool includeTimestamp)
        {
            addSubscriptionNameLabel(labels);

            await _systemMetricsPublisher.WriteGaugeMeasurementAsync(name, description, value, labels, includeTimestamp);
        }

        private void addSubscriptionNameLabel(Dictionary<string, string> labels)
        {
            if (!labels.ContainsKey("subscription_id")) return;

            string nameValue;
            if (_subscriptionMappings.TryGetValue(labels["subscription_id"], out nameValue))
            {
                labels["subscription_name"] = nameValue;
            }
            else
            {
                labels["subscription_name"] = "";
            }
        }
    }
}
