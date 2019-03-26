using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    public class GenericMetricValidator : IMetricValidator<GenericAzureMetricDefinition>
    {
        public List<string> Validate(GenericAzureMetricDefinition genericMetricDefinition)
        {
            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(genericMetricDefinition.ResourceUri))
            {
                errorMessages.Add("No resource uri is configured");
            }

            return errorMessages;
        }
    }
}