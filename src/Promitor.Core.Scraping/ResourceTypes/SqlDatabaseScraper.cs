using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    /// <summary>
    /// Scrapes an Azure SQL Database.
    /// </summary>
    public class SqlDatabaseScraper : AzureMonitorScraper<SqlDatabaseResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Sql/servers/{2}/databases/{3}";

        public SqlDatabaseScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, SqlDatabaseResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, scrapeDefinition.ResourceGroupName, resource.ServerName, resource.DatabaseName);
        }

        protected override Dictionary<string, string> DetermineMetricLabels(SqlDatabaseResourceDefinition resourceDefinition)
        {
            var metricLabels = base.DetermineMetricLabels(resourceDefinition);

            metricLabels.TryAdd("server", resourceDefinition.ServerName);
            metricLabels.TryAdd("database", resourceDefinition.DatabaseName);

            return metricLabels;
        }
    }
}