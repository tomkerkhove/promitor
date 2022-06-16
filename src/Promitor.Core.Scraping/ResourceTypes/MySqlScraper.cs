using System;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Contracts.ResourceTypes.Enums;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class MySqlScraper : AzureMonitorScraper<MySqlResourceDefinition>
    {
        private const string SingleServerResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.DBforMySQL/servers/{2}";
        private const string FlexibleServerResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.DBforMySQL/flexibleServers/{2}";

        public MySqlScraper(ScraperConfiguration scraperConfiguration)
           : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, MySqlResourceDefinition resource)
        {
            var serverDefinition = (MySqlResourceDefinition) scrapeDefinition.Resource;
            switch (serverDefinition.Type)
            {
                case MySqlServerType.Single:
                    return string.Format(SingleServerResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.ServerName);
                case MySqlServerType.Flexible:
                    return string.Format(FlexibleServerResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.ServerName);
                default:
                    throw new ArgumentOutOfRangeException($"Server type '{serverDefinition.Type}' is not supported for now");
            }
        }
    }
}
