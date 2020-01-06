using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class SqlManagedInstanceScraper : Scraper<SqlManagedInstanceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Sql/managedInstances/{2}";

        /// <summary>
        ///     Initializes an instance of the <see cref="SqlManagedInstanceScraper" /> class.
        /// </summary>
        /// <param name="scraperConfiguration">The scraper configuration</param>
        public SqlManagedInstanceScraper(ScraperConfiguration scraperConfiguration) : base(scraperConfiguration)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, ScrapeDefinition<AzureResourceDefinition> scrapeDefinition, SqlManagedInstanceDefinition resourceDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(
                ResourceUriTemplate,
                AzureMetadata.SubscriptionId,
                scrapeDefinition.ResourceGroupName,
                resourceDefinition.InstanceName);

            var metricName = scrapeDefinition.AzureMetricConfiguration.MetricName;
            var dimensionName = scrapeDefinition.AzureMetricConfiguration.Dimension?.Name;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, dimensionName, aggregationType, aggregationInterval, resourceUri);

            return new ScrapeResult(subscriptionId, scrapeDefinition.ResourceGroupName, null, resourceUri, foundMetricValue);
        }
    }
}