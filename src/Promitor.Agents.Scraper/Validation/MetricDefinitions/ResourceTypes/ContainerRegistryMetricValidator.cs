using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    internal class ContainerRegistryMetricValidator : IMetricValidator
    {
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<ContainerRegistryResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.RegistryName))
                {
                    yield return "No registry name is configured";
                }
            }
        }
    }
}