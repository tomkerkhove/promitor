using System;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Contracts.ResourceTypes.Enums;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class PostgreSqlScraper : AzureMonitorScraper<PostgreSqlResourceDefinition>
    {
        private const string SingleServerResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.DBforPostgreSQL/servers/{2}";
        private const string FlexibleServerResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.DBforPostgreSQL/flexibleServers/{2}";
        private const string HyperscaleServerResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.DBforPostgreSQL/serverGroupsv2/{2}";

        public PostgreSqlScraper(ScraperConfiguration scraperConfiguration)
           : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, PostgreSqlResourceDefinition resource)
        {
            var serverDefinition = (PostgreSqlResourceDefinition) scrapeDefinition.Resource;
            switch (serverDefinition.Type)
            {
                case PostgreSqlServerType.Single:
                    return string.Format(SingleServerResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.ServerName);
                case PostgreSqlServerType.Flexible:
                    return string.Format(FlexibleServerResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.ServerName);
                case PostgreSqlServerType.Hyperscale:
                    return string.Format(HyperscaleServerResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.ServerName);
                case PostgreSqlServerType.Arc:
                default:
                    throw new ArgumentOutOfRangeException($"Server type '{serverDefinition.Type}' is not supported for now");
            }
        }
    }
}
