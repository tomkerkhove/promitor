using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    internal class MonitorAutoscaleMetricValidator : IMetricValidator
    {
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<MonitorAutoscaleResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.AutoscaleSettingsName))
                {
                    yield return "No Azure Monitor Autoscale settings name is configured";
                }
            }
        }
    }
}
