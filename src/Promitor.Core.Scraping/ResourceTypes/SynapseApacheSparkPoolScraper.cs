using System.Collections.Generic;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    /// <summary>
    ///     Scrapes an Apache Spark pool in an Azure Synapse workspace.
    /// </summary>
    public class SynapseApacheSparkPoolScraper : AzureMonitorScraper<SynapseApacheSparkPoolResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Synapse/workspaces/{2}/bigDataPools/{3}";

        public SynapseApacheSparkPoolScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, SynapseApacheSparkPoolResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.WorkspaceName, resource.PoolName);
        }

        protected override Dictionary<string, string> DetermineMetricLabels(SynapseApacheSparkPoolResourceDefinition resourceDefinition)
        {
            var metricLabels = base.DetermineMetricLabels(resourceDefinition);

            metricLabels.TryAdd("workspace_name", resourceDefinition.WorkspaceName);
            metricLabels.TryAdd("pool_name", resourceDefinition.PoolName);

            return metricLabels;
        }
    }
}