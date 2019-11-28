using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    /// <summary>
    /// Scrapes an Azure SQL Database.
    /// </summary>
    public class SqlDatabaseScraper : Scraper<SqlDatabaseResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Sql/servers/{2}/databases/{3}";

        /// <summary>
        /// Initializes an instance of the <see cref="SqlDatabaseScraper" /> class.
        /// </summary>
        /// <param name="scraperConfiguration">The scraper configuration</param>
        public SqlDatabaseScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, ScrapeDefinition<AzureResourceDefinition> scrapeDefinition, SqlDatabaseResourceDefinition resource, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(
                ResourceUriTemplate,
                AzureMetadata.SubscriptionId,
                scrapeDefinition.ResourceGroupName,
                resource.ServerName,
                resource.DatabaseName);

            var metricName = scrapeDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri);

            var labels = new Dictionary<string, string>
            {
                {"server", resource.ServerName},
                {"database", resource.DatabaseName}
            };

            return new ScrapeResult(subscriptionId, scrapeDefinition.ResourceGroupName, null, resourceUri, foundMetricValue, labels);
        }
    }
}