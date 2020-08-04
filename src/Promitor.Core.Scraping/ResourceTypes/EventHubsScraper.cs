using System;
using System.Collections.Generic;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class EventHubsScraper : AzureMonitorScraper<EventHubResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.EventHub/namespaces/{2}";

        public EventHubsScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, EventHubResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.Namespace);
        }

        protected override Dictionary<string, string> DetermineMetricLabels(EventHubResourceDefinition resourceDefinition)
        {
            var metricLabels = base.DetermineMetricLabels(resourceDefinition);

            //metricLabels.TryAdd("entity_name", resourceDefinition.TopicName);

            return metricLabels;
        }

        protected override string DetermineMetricDimension(MetricDimension dimension)
        {
            if (dimension?.Name.Equals("EntityName", StringComparison.InvariantCultureIgnoreCase)==false)
            {
                return base.DetermineMetricDimension(dimension);
            }

            return "EntityName";
        }


        protected override string DetermineMetricFilter(EventHubResourceDefinition resourceDefinition)
        {
            var entityName = "*";

            if(string.IsNullOrWhiteSpace(resourceDefinition.TopicName))
            {
                entityName = resourceDefinition.TopicName;
            }

            return $"EntityName eq '{entityName}'";
        }
    }
}