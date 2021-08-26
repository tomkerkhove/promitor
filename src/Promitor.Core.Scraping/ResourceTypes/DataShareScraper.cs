using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class DataShareScraper : AzureMonitorScraper<DataShareResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.DataShare/accounts/{2}";

        public DataShareScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, DataShareResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.AccountName);
        }

        protected override string DetermineMetricFilter(string metricName, DataShareResourceDefinition resourceDefinition)
        {
            var fieldName = GetMetricFilterFieldName(metricName);

            var entityName = "*";

            if (IsShareNameConfigured(resourceDefinition))
            {
                entityName = resourceDefinition.ShareName;
            }

            return $"{fieldName} eq '{entityName}'";
        }

        protected override string DetermineMetricDimension(string metricName, DataShareResourceDefinition resourceDefinition, MetricDimension dimension)
        {
            if (IsShareNameConfigured(resourceDefinition))
            {
                return base.DetermineMetricDimension(metricName, resourceDefinition, dimension);
            }

            var dimensionName = GetMetricFilterFieldName(metricName);
            Logger.LogTrace($"Using '{dimensionName}' dimension since no share name was configured.");

            return dimensionName;
        }

        protected override List<MeasuredMetric> EnrichMeasuredMetrics(DataShareResourceDefinition resourceDefinition, string dimensionName, List<MeasuredMetric> metricValues)
        {
            // Change Azure Monitor dimension name to more representable value
            foreach (var measuredMetric in metricValues.Where(metricValue => metricValue.DimensionName == "ShareName"
                                                                             || metricValue.DimensionName == "ShareSubscriptionName"))
            {
                measuredMetric.DimensionName = "share_name";
            }

            return metricValues;
        }

        private static string GetMetricFilterFieldName(string metricName)
        {
            var fieldName = "ShareName";

            // We need to switch field names when querying activities
            if (metricName.Equals("ShareSubscriptionCount", StringComparison.InvariantCultureIgnoreCase))
            {
                fieldName = "ShareSubscriptionName";
            }

            return fieldName;
        }

        protected override Dictionary<string, string> DetermineMetricLabels(DataShareResourceDefinition resourceDefinition)
        {
            var metricLabels = base.DetermineMetricLabels(resourceDefinition);

            if (IsShareNameConfigured(resourceDefinition))
            {
                metricLabels.Add("share_name", resourceDefinition.ShareName);
            }

            return metricLabels;
        }

        private static bool IsShareNameConfigured(DataShareResourceDefinition resourceDefinition)
        {
            return string.IsNullOrWhiteSpace(resourceDefinition.ShareName) == false;
        }
    }
}