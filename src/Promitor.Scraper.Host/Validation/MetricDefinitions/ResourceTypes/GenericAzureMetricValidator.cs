using System.Collections.Generic;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class GenericAzureMetricValidator : MetricValidator<GenericAzureMetricDefinition>
    {
        protected override IEnumerable<string> Validate(GenericAzureMetricDefinition genericMetricDefinition)
        {
            Guard.NotNull(genericMetricDefinition, nameof(genericMetricDefinition));

            if (string.IsNullOrWhiteSpace(genericMetricDefinition.ResourceUri))
            {
                yield return "No resource uri is configured";
            }
        }
    }
}