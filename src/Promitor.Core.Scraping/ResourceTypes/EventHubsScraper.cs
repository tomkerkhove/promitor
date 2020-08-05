using System;
using System.Collections.Generic;
using System.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class EventHubsScraper : AzureMonitorScraper<EventHubResourceDefinition>
    {
        private const string EntityNameLabel = "entity_name";
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.EventHub/namespaces/{2}";

        public EventHubsScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, EventHubResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.Namespace);
        }

        protected override List<MeasuredMetric> EnrichMeasuredMetrics(EventHubResourceDefinition resourceDefinition, string dimensionName, List<MeasuredMetric> metricValues)
        {
            // Change Azure Monitor Dimension name to more representable value
            foreach (var measuredMetric in metricValues.Where(metricValue=> string.IsNullOrWhiteSpace(metricValue.DimensionName) == false))
            {
                measuredMetric.DimensionName = EntityNameLabel;
            }

            return metricValues;
        }

        protected override Dictionary<string, string> DetermineMetricLabels(EventHubResourceDefinition resourceDefinition)
        {
            var metricLabels = base.DetermineMetricLabels(resourceDefinition); 
            
            if (IsTopicDeclared(resourceDefinition))
            {
                metricLabels.Add(EntityNameLabel, resourceDefinition.TopicName);
            }
            
            return metricLabels;
        }

        protected override string DetermineMetricDimension(EventHubResourceDefinition resourceDefinition, MetricDimension dimension)
        {
            if (IsTopicDeclared(resourceDefinition))
            {
                return base.DetermineMetricDimension(resourceDefinition, dimension);
            }

            // TODO: Log that we are ignoring
            return "EntityName";
        }
        
        protected override string DetermineMetricFilter(EventHubResourceDefinition resourceDefinition)
        {
            var entityName = "*";

            if(IsTopicDeclared(resourceDefinition))
            {
                entityName = resourceDefinition.TopicName;
            }

            return $"EntityName eq '{entityName}'";
        }

        private static bool IsTopicDeclared(EventHubResourceDefinition resourceDefinition)
        {
            return string.IsNullOrWhiteSpace(resourceDefinition.TopicName) == false;
        }
    }
}