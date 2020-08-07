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
    public class ServiceBusQueueScraper : AzureMonitorScraper<ServiceBusQueueResourceDefinition>
    {
        private const string EntityNameLabel = "entity_name";
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ServiceBus/namespaces/{2}";

        public ServiceBusQueueScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, ServiceBusQueueResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.Namespace);
        }

        protected override List<MeasuredMetric> EnrichMeasuredMetrics(ServiceBusQueueResourceDefinition resourceDefinition, string dimensionName, List<MeasuredMetric> metricValues)
        {
            // Change Azure Monitor Dimension name to more representable value
            foreach (var measuredMetric in metricValues.Where(metricValue => string.IsNullOrWhiteSpace(metricValue.DimensionName) == false))
            {
                measuredMetric.DimensionName = EntityNameLabel;
            }

            return metricValues;
        }

        protected override Dictionary<string, string> DetermineMetricLabels(ServiceBusQueueResourceDefinition resourceDefinition)
        {
            var metricLabels = base.DetermineMetricLabels(resourceDefinition);

            if (IsQueueDeclared(resourceDefinition))
            {
                metricLabels.Add(EntityNameLabel, resourceDefinition.QueueName);
            }

            return metricLabels;
        }

        protected override string DetermineMetricDimension(ServiceBusQueueResourceDefinition resourceDefinition, MetricDimension dimension)
        {
            if (IsQueueDeclared(resourceDefinition))
            {
                return base.DetermineMetricDimension(resourceDefinition, dimension);
            }

            Logger.LogWarning("Using 'EntityName' dimension since no topic was configured.");

            return "EntityName";
        }

        protected override string DetermineMetricFilter(ServiceBusQueueResourceDefinition resourceDefinition)
        {
            var entityName = "*";

            if (IsQueueDeclared(resourceDefinition))
            {
                entityName = resourceDefinition.QueueName;
            }

            return $"EntityName eq '{entityName}'";
        }

        private static bool IsQueueDeclared(ServiceBusQueueResourceDefinition resourceDefinition)
        {
            return string.IsNullOrWhiteSpace(resourceDefinition.QueueName) == false;
        }
    }
}