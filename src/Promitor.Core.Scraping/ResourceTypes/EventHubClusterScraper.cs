using System;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Interfaces;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class EventHubClusterScraper : AzureMonitorScraper<IAzureResourceDefinition>
    {
        public EventHubClusterScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, IAzureResourceDefinition resource)
        {
            var eventHubClusterResource = resource as EventHubClusterResourceDefinition;
            if (eventHubClusterResource == null)
            {
                throw new ArgumentException("Invalid resource type", nameof(resource));
            }

            return $"subscriptions/{subscriptionId}/resourceGroups/{eventHubClusterResource.ResourceGroupName}/providers/Microsoft.EventHub/clusters/{eventHubClusterResource.ClusterName}";
        }
    }
}