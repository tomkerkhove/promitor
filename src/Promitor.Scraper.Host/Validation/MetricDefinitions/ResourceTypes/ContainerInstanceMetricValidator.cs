using System.Collections.Generic;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class ContainerInstanceMetricValidator : MetricValidator<ContainerInstanceMetricDefinition>
    {
        protected override IEnumerable<string> Validate(ContainerInstanceMetricDefinition containerInstanceMetricDefinition)
        {
            Guard.NotNull(containerInstanceMetricDefinition, nameof(containerInstanceMetricDefinition));

            if (string.IsNullOrWhiteSpace(containerInstanceMetricDefinition.ContainerGroup))
            {
                yield return "No container group is configured";
            }
        }
    }
}