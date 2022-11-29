using System;
using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;

namespace Promitor.Integrations.Sinks.Core
{
    public class MetricSink
    {
        protected ILogger Logger { get; }
        protected IMetricsDeclarationProvider MetricsDeclarationProvider { get; }

        protected MetricSink(IMetricsDeclarationProvider metricsDeclarationProvider, ILogger logger)
        {
            Guard.NotNull(metricsDeclarationProvider, nameof(metricsDeclarationProvider));
            Guard.NotNull(logger, nameof(logger));

            MetricsDeclarationProvider = metricsDeclarationProvider;
            Logger = logger;
        }

        public Dictionary<string, string> DetermineLabels(string metricName, ScrapeResult scrapeResult, MeasuredMetric measuredMetric, Func<string, string> mutateLabelName = null)
        {
            var metricDefinition = MetricsDeclarationProvider.GetPrometheusDefinition(metricName);
            var defaultLabels = MetricsDeclarationProvider.GetDefaultLabels();

            var labels = new Dictionary<string, string>(scrapeResult.Labels.Select(label =>
            {
                var labelName = DetermineLabelName(label.Value, mutateLabelName);
                return new KeyValuePair<string, string>(labelName, label.Value);
            }));

            if (measuredMetric.IsDimensional)
            {
                labels.Add(DetermineLabelName(measuredMetric.DimensionName, mutateLabelName), measuredMetric.DimensionValue);
            }

            if (metricDefinition?.Labels?.Any() == true)
            {
                foreach (var customLabel in metricDefinition.Labels)
                {
                    var customLabelKey = DetermineLabelName(customLabel.Key, mutateLabelName);
                    if (labels.ContainsKey(customLabelKey))
                    {
                        Logger.LogWarning("Custom label {CustomLabelName} was already specified with value '{LabelValue}' instead of '{CustomLabelValue}'. Ignoring...", customLabel.Key, labels[customLabelKey], customLabel.Value);
                        continue;
                    }

                    labels.Add(customLabelKey, customLabel.Value);
                }
            }

            foreach (var defaultLabel in defaultLabels)
            {
                var defaultLabelKey = DetermineLabelName(defaultLabel.Key, mutateLabelName);
                if (labels.ContainsKey(defaultLabelKey) == false)
                {
                    labels.Add(defaultLabelKey, defaultLabel.Value);
                }
            }

            // Add the tenant id
            var metricsDeclaration = MetricsDeclarationProvider.Get(applyDefaults: true);
            if (labels.ContainsKey("tenant_id") == false)
            {
                labels.Add("tenant_id", metricsDeclaration.AzureMetadata.TenantId);
            }

            var orderedLabels = labels.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return orderedLabels;
        }

        private static string DetermineLabelName(string originalLabelName, Func<string, string> mutateLabelName)
        {
            return mutateLabelName!= null ? mutateLabelName.Invoke(originalLabelName) : originalLabelName;
        }
    }
}