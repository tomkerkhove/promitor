using System.Collections.Generic;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class ContainerRegistryMetricValidator : MetricValidator<ContainerRegistryMetricDefinition>
    {
        protected override IEnumerable<string> Validate(ContainerRegistryMetricDefinition containerRegistryMetricDefinition)
        {
            Guard.NotNull(containerRegistryMetricDefinition, nameof(containerRegistryMetricDefinition));

            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(containerRegistryMetricDefinition.RegistryName))
            {
                errorMessages.Add("No registry name is configured");
            }

            return errorMessages;
        }
    }
}