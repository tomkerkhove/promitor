using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class DataFactoryScraper : AzureMonitorScraper<DataFactoryResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.DataFactory/factories/{2}";

        public DataFactoryScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, DataFactoryResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.FactoryName);
        }

        protected override string DetermineMetricFilter(string metricName, DataFactoryResourceDefinition resourceDefinition)
        {
            var fieldName = GetMetricFilterFieldName(metricName);

            var entityName = "*";

            if (IsPipelineNameConfigured(resourceDefinition))
            {
                entityName = resourceDefinition.PipelineName;
            }

            return $"{fieldName} eq '{entityName}'";
        }

        protected override Dictionary<string, string> DetermineMetricLabels(DataFactoryResourceDefinition resourceDefinition)
        {
            var metricLabels = base.DetermineMetricLabels(resourceDefinition);

            if (IsPipelineNameConfigured(resourceDefinition))
            {
                metricLabels.Add("pipeline_name", resourceDefinition.PipelineName);
            }

            return metricLabels;
        }

        protected override List<string> DetermineMetricDimensions(string metricName, DataFactoryResourceDefinition resourceDefinition, AzureMetricConfiguration configuration)
        {
            if (IsPipelineNameConfigured(resourceDefinition))
            {
                return base.DetermineMetricDimensions(metricName, resourceDefinition, configuration);
            }

            var dimensionName = GetMetricFilterFieldName(metricName);
            Logger.LogTrace($"Using '{dimensionName}' dimension since no pipeline name was configured.");

            return new List<string> { dimensionName };
        }

        private static bool IsPipelineNameConfigured(DataFactoryResourceDefinition resourceDefinition)
        {
            return string.IsNullOrWhiteSpace(resourceDefinition.PipelineName) == false;
        }

        private static string GetMetricFilterFieldName(string metricName)
        {
            var fieldName = "Name";

            // We need to switch field names when querying activities
            if (metricName.Equals("ActivitySucceededRuns", StringComparison.InvariantCultureIgnoreCase)
                || metricName.Equals("ActivityFailedRuns", StringComparison.InvariantCultureIgnoreCase)
                || metricName.Equals("ActivityCancelledRuns", StringComparison.InvariantCultureIgnoreCase))
            {
                fieldName = "PipelineName";
            }

            return fieldName;
        }
    }
}