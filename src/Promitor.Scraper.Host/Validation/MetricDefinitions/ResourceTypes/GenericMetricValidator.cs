using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResouceTypes;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    public class GenericMetricValidator : IMetricValidator<GenericMetricDefinition>
    {
        public List<string> Validate(GenericMetricDefinition genericMetricDefinition)
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