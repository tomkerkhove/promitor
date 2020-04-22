using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    internal class ContainerInstanceMetricValidator : IMetricValidator
    {
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var definition in metricDefinition.Resources.Cast<ContainerInstanceResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(definition.ContainerGroup))
                {
                    yield return "No container group is configured";
                }
            }
        }
    }
}