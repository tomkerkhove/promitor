using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    /// <summary>
    ///     Scrapes an Azure SQL Server instance.
    /// </summary>
    public class SqlServerScraper : AzureMonitorScraper<SqlServerResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Sql/servers/{2}";

        public SqlServerScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, SqlServerResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, scrapeDefinition.ResourceGroupName, resource.ServerName);
        }

        protected override Dictionary<string, string> DetermineMetricLabels(SqlServerResourceDefinition resourceDefinition)
        {
            var metricLabels = base.DetermineMetricLabels(resourceDefinition);

            metricLabels.TryAdd("server", resourceDefinition.ServerName);

            return metricLabels;
        }
    }
}