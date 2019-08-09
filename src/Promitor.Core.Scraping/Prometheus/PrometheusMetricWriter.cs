using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus.Client;
using Promitor.Core.Configuration;
using Promitor.Core.Configuration.FeatureFlags;
using Promitor.Core.Configuration.Model.Prometheus;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Prometheus.Interfaces;

namespace Promitor.Core.Scraping.Prometheus
{
    public class PrometheusMetricWriter : IPrometheusMetricWriter
    {
        private readonly FeatureToggleClient _featureToggleClient;
        private readonly IOptionsMonitor<PrometheusConfiguration> _prometheusConfiguration;
        private readonly ILogger<PrometheusMetricWriter> _logger;

        public PrometheusMetricWriter(FeatureToggleClient featureToggleClient, IOptionsMonitor<PrometheusConfiguration> prometheusConfiguration, ILogger<PrometheusMetricWriter> logger)
        {
            Guard.NotNull(featureToggleClient, nameof(featureToggleClient));
            Guard.NotNull(prometheusConfiguration, nameof(prometheusConfiguration));
            Guard.NotNull(logger, nameof(logger));

            _featureToggleClient = featureToggleClient;
            _prometheusConfiguration = prometheusConfiguration;
            _logger = logger;
        }

        public void ReportMetric(MetricDefinition metricDefinition, ScrapeResult scrapedMetricResult)
        {
            var metricsTimestampFeatureFlag = _featureToggleClient.IsActive(ToggleNames.DisableMetricTimestamps, defaultFlagState: true);

            var labels = DetermineLabels(metricDefinition, scrapedMetricResult);

            var gauge = Metrics.CreateGauge(metricDefinition.Name, metricDefinition.Description, includeTimestamp: metricsTimestampFeatureFlag, labelNames: labels.Names);
            var metricValue = DetermineMetricMeasurement(scrapedMetricResult);
            gauge.WithLabels(labels.Values).Set(metricValue);
        }

        private double DetermineMetricMeasurement(ScrapeResult scrapedMetricResult)
        {
            var metricUnavailableValue = _prometheusConfiguration.CurrentValue?.MetricUnavailableValue ?? Defaults.Prometheus.MetricUnavailableValue;
            return scrapedMetricResult.MetricValue ?? metricUnavailableValue;
        }

        private (string[] Names, string[] Values) DetermineLabels(MetricDefinition metricDefinition, ScrapeResult scrapeResult)
        {
            var labels = new Dictionary<string, string>(scrapeResult.Labels);

            if (metricDefinition?.Labels?.Any() == true)
            {
                foreach (var customLabel in metricDefinition.Labels)
                {
                    if (labels.ContainsKey(customLabel.Key))
                    {
                        _logger.LogWarning("Custom label '{CustomLabelName}' was already specified with value 'LabelValue' instead of 'CustomLabelValue'. Ignoring...", customLabel.Key, labels[customLabel.Key], customLabel.Value);
                        continue;
                    }

                    labels.Add(customLabel.Key, customLabel.Value);
                }
            }

            return (labels.Keys.ToArray(), labels.Values.ToArray());
        }
    }
}
