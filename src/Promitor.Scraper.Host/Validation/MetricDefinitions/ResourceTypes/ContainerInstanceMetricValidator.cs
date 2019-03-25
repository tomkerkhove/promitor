using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    public class ContainerInstanceMetricValidator : IMetricValidator<ContainerInstanceMetricDefinition>
    {
        public List<string> Validate(ContainerInstanceMetricDefinition serviceBusQueueMetricDefinition)
        {
            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(serviceBusQueueMetricDefinition.ContainerGroup))
            {
                errorMessages.Add("No container group is configured");
            }

            return errorMessages;
        }
    }
}