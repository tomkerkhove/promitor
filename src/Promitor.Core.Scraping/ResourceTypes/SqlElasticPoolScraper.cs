using System.Collections.Generic;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    /// <summary>
    ///     Scrapes an Azure SQL Database.
    /// </summary>
    public class SqlElasticPoolScraper : AzureMonitorScraper<SqlElasticPoolResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Sql/servers/{2}/elasticPools/{3}";

        public SqlElasticPoolScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, SqlElasticPoolResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.ServerName, resource.PoolName);
        }

        protected override Dictionary<string, string> DetermineMetricLabels(SqlElasticPoolResourceDefinition resourceDefinition)
        {
            var metricLabels = base.DetermineMetricLabels(resourceDefinition);

            metricLabels.TryAdd("server", resourceDefinition.ServerName);
            metricLabels.TryAdd("elastic_pool", resourceDefinition.PoolName);

            return metricLabels;
        }
    }
}