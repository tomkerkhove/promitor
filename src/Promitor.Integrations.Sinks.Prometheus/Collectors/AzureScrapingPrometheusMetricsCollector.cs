using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Microsoft.Extensions.Options;
using Promitor.Core.Metrics.Prometheus.Collectors.Interfaces;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Integrations.Sinks.Prometheus.Configuration;

namespace Promitor.Integrations.Sinks.Prometheus.Collectors
{
    public class AzureScrapingPrometheusMetricsCollector : IAzureScrapingPrometheusMetricsCollector
    {
        private readonly IPrometheusMetricsCollector _prometheusMetricsCollector;
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        private readonly IOptionsMonitor<PrometheusScrapingEndpointSinkConfiguration> _prometheusConfiguration;

        public AzureScrapingPrometheusMetricsCollector(IMetricsDeclarationProvider metricsDeclarationProvider, IPrometheusMetricsCollector prometheusMetricsCollector, IOptionsMonitor<PrometheusScrapingEndpointSinkConfiguration> prometheusConfiguration)
        {
            Guard.NotNull(metricsDeclarationProvider, nameof(metricsDeclarationProvider));
            Guard.NotNull(prometheusMetricsCollector, nameof(prometheusMetricsCollector));

            _prometheusConfiguration = prometheusConfiguration;
            _prometheusMetricsCollector = prometheusMetricsCollector;
            _metricsDeclarationProvider = metricsDeclarationProvider;
        }

        /// <summary>
        /// Sets a new value for a measurement on a gauge
        /// </summary>
        /// <param name="name">Name of the metric</param>
        /// <param name="description">Description of the metric</param>
        /// <param name="value">New measured value</param>
        /// <param name="labels">Labels that are applicable for this measurement</param>
        public void WriteGaugeMeasurement(string name, string description, double value, Dictionary<string, string> labels)
        {
            var enableMetricTimestamps = _prometheusConfiguration.CurrentValue.EnableMetricTimestamps;

            var metricsDeclaration = _metricsDeclarationProvider.Get(applyDefaults: true);
            if (labels.ContainsKey("tenant_id") == false)
            {
                labels.Add("tenant_id", metricsDeclaration.AzureMetadata.TenantId);
            }

            var orderedLabels = labels.OrderByDescending(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            _prometheusMetricsCollector.WriteGaugeMeasurement(name, description, value, orderedLabels, enableMetricTimestamps);
        }
    }
}
