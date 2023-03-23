using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    internal class TrafficManagerMetricValidator : IMetricValidator
    {
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<TrafficManagerResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.Name))
                {
                    yield return "No Azure Traffic Manager profile name is configured";
                }
            }
        }
    }
}