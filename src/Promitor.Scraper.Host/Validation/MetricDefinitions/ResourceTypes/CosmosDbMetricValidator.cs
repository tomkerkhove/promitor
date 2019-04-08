﻿using System.Collections.Generic;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class CosmosDbMetricValidator : MetricValidator<CosmosDbMetricDefinition>
    {
        protected override IEnumerable<string> Validate(CosmosDbMetricDefinition cosmosDbMetricDefinition)
        {
            Guard.NotNull(cosmosDbMetricDefinition, nameof(cosmosDbMetricDefinition));

            if (string.IsNullOrWhiteSpace(cosmosDbMetricDefinition.DbName))
            {
                yield return "No Cosmos DB name is configured";
            }
        }
    }
}