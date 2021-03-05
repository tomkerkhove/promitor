using System.Collections.Generic;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    /// <summary>
    ///     Scrapes an Azure Synapse workspace.
    /// </summary>
    public class SynapseWorkspaceScraper : AzureMonitorScraper<SynapseWorkspaceResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Synapse/workspaces/{2}";

        public SynapseWorkspaceScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, SynapseWorkspaceResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.WorkspaceName);
        }

        protected override Dictionary<string, string> DetermineMetricLabels(SynapseWorkspaceResourceDefinition resourceDefinition)
        {
            var metricLabels = base.DetermineMetricLabels(resourceDefinition);

            metricLabels.TryAdd("workspace_name", resourceDefinition.WorkspaceName);

            return metricLabels;
        }
    }
}