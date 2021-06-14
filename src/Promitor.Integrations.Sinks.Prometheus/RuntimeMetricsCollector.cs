using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Microsoft.Extensions.Options;
using Prometheus.Client;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Integrations.Sinks.Prometheus.Configuration;

namespace Promitor.Integrations.Sinks.Prometheus
{
    public class RuntimeMetricsCollector : IRuntimeMetricsCollector
    {
        private readonly IMetricFactory _metricFactory;
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        private readonly IOptionsMonitor<PrometheusScrapingEndpointSinkConfiguration> _prometheusConfiguration;

        public RuntimeMetricsCollector(IMetricsDeclarationProvider metricsDeclarationProvider, IMetricFactory metricFactory, IOptionsMonitor<PrometheusScrapingEndpointSinkConfiguration> prometheusConfiguration)
        {
            Guard.NotNull(metricsDeclarationProvider, nameof(metricsDeclarationProvider));
            Guard.NotNull(metricFactory, nameof(metricFactory));

            _metricFactory = metricFactory;
            _prometheusConfiguration = prometheusConfiguration;
            _metricsDeclarationProvider = metricsDeclarationProvider;
        }

        /// <summary>
        /// Sets a new value for a measurement on a gauge
        /// </summary>
        /// <param name="name">Name of the metric</param>
        /// <param name="description">Description of the metric</param>
        /// <param name="value">New measured value</param>
        /// <param name="labels">Labels that are applicable for this measurement</param>
        public void SetGaugeMeasurement(string name, string description, double value, Dictionary<string, string> labels)
        {
            var enableMetricTimestamps = _prometheusConfiguration.CurrentValue.EnableMetricTimestamps;

            var metricsDeclaration = _metricsDeclarationProvider.Get(applyDefaults: true);
            if (labels.ContainsKey("tenant_id") == false)
            {
                labels.Add("tenant_id", metricsDeclaration.AzureMetadata.TenantId);
            }

            var orderedLabels = labels.OrderBy(kvp => kvp.Key).ToDictionary(kvp=>kvp.Key, kvp=>kvp.Value);

            var gauge = _metricFactory.CreateGauge(name, help: description, includeTimestamp: enableMetricTimestamps, labelNames: orderedLabels.Keys.ToArray());
            gauge.WithLabels(orderedLabels.Values.ToArray()).Set(value);
        }
    }
}
