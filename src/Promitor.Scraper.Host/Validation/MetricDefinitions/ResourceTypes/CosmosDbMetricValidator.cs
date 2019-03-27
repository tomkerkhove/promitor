using System.Collections.Generic;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class CosmosDbMetricValidator : MetricValidator<CosmosDbMetricDefinition>
    {
        protected override IEnumerable<string> Validate(CosmosDbMetricDefinition cosmosDbMetricDefinition)
        {
            Guard.NotNull(cosmosDbMetricDefinition, nameof(cosmosDbMetricDefinition));

            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(cosmosDbMetricDefinition.DbName))
            {
                errorMessages.Add("No Cosmos db Name is configured");
            }

            return errorMessages;
        }
    }
}