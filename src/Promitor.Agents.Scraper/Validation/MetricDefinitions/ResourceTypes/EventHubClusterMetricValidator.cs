using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    internal class EventHubClusterMetricValidator : IMetricValidator
    {
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            var errorMessages = new List<string>();

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<EventHubClusterResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.ClusterName))
                {
                    errorMessages.Add("No Azure Event Hub ClusterName is configured");
                }
            }

            return errorMessages;
        }
    }
}