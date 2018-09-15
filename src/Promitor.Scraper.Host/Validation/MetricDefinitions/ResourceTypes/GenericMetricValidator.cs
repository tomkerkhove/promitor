using System.Collections.Generic;
using Promitor.Scraper.Host.Configuration.Model.Metrics.ResouceTypes;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    public class GenericMetricValidator : IMetricValidator<GenericMetricDefinition>
    {
        public List<string> Validate(GenericMetricDefinition genericMetricDefinition)
        {
            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(genericMetricDefinition.Filter))
            {
                errorMessages.Add("No filter is configured");
            }

            if (string.IsNullOrWhiteSpace(genericMetricDefinition.ResourceUri))
            {
                errorMessages.Add("No resource uri is configured");
            }

            return errorMessages;
        }
    }
}