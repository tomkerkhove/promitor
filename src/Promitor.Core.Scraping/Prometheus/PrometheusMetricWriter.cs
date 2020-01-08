using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus.Client;
using Promitor.Core.Configuration;
using Promitor.Core.Configuration.Model.Prometheus;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Prometheus.Interfaces;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Core.Scraping.Prometheus
{
    public class PrometheusMetricWriter : IPrometheusMetricWriter
    {
        private readonly IOptionsMonitor<PrometheusConfiguration> _prometheusConfiguration;
        private readonly ILogger<PrometheusMetricWriter> _logger;

        public PrometheusMetricWriter(IOptionsMonitor<PrometheusConfiguration> prometheusConfiguration, ILogger<PrometheusMetricWriter> logger)
        {
            Guard.NotNull(prometheusConfiguration, nameof(prometheusConfiguration));
            Guard.NotNull(logger, nameof(logger));

            _prometheusConfiguration = prometheusConfiguration;
            _logger = logger;
        }

        public void ReportMetric(PrometheusMetricDefinition metricDefinition, ScrapeResult scrapedMetricResult)
        {
            var enableMetricTimestamps = _prometheusConfiguration.CurrentValue.EnableMetricTimestamps;

            foreach (var measuredMetric in scrapedMetricResult.MetricValues)
            {
                var measuredMetricValue = DetermineMetricMeasurement(measuredMetric);
                var labels = DetermineLabels(metricDefinition, scrapedMetricResult, measuredMetric);
                
                var gauge = Metrics.CreateGauge(metricDefinition.Name, metricDefinition.Description, includeTimestamp: enableMetricTimestamps, labelNames: labels.Names);
                gauge.WithLabels(labels.Values).Set(measuredMetricValue);
            }
        }

        private double DetermineMetricMeasurement(MeasuredMetric scrapedMetricResult)
        {
            var metricUnavailableValue = _prometheusConfiguration.CurrentValue?.MetricUnavailableValue ?? Defaults.Prometheus.MetricUnavailableValue;
            return scrapedMetricResult.Value ?? metricUnavailableValue;
        }

        private (string[] Names, string[] Values) DetermineLabels(PrometheusMetricDefinition metricDefinition, ScrapeResult scrapeResult, MeasuredMetric measuredMetric)
        {
            var labels = new Dictionary<string, string>(scrapeResult.Labels);

            if (measuredMetric.IsDimensional)
            {
                labels.Add(measuredMetric.DimensionName.ToLower(), measuredMetric.DimensionValue);
            }

            if (metricDefinition?.Labels?.Any() == true)
            {
                foreach (var customLabel in metricDefinition.Labels)
                {
                    if (labels.ContainsKey(customLabel.Key))
                    {
                        _logger.LogWarning("Custom label {CustomLabelName} was already specified with value 'LabelValue' instead of 'CustomLabelValue'. Ignoring...", customLabel.Key, labels[customLabel.Key], customLabel.Value);
                        continue;
                    }

                    labels.Add(customLabel.Key, customLabel.Value);
                }
            }

            return (labels.Keys.ToArray(), labels.Values.ToArray());
        }
    }
}
