using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Promitor.Scraper.Model;
using Promitor.Scraper.Model.Configuration;
using Promitor.Scraper.Model.Configuration.Metrics;
using Promitor.Scraper.Scraping.Interfaces;

namespace Promitor.Scraper.Scraping
{
    public abstract class Scraper<TMetricDefinition> : IScraper<MetricDefinition> where TMetricDefinition : MetricDefinition, new()
    {
        protected Scraper(AzureMetadata azureMetadata, AzureCredentials azureCredentials)
        {
            AzureMetadata = azureMetadata ?? throw new ArgumentNullException(nameof(azureMetadata));
            AzureCredentials = azureCredentials ?? throw new ArgumentNullException(nameof(azureCredentials));
        }

        /// <summary>
        ///     Credentials used to authenticate to Microsoft Azure
        /// </summary>
        protected AzureCredentials AzureCredentials { get; }

        /// <summary>
        ///     Metadata concerning the Azure resources
        /// </summary>
        protected AzureMetadata AzureMetadata { get; }

        public async Task ScrapeAsync(MetricDefinition metricDefinition)
        {
            if (metricDefinition == null)
            {
                throw new ArgumentNullException(nameof(metricDefinition));
            }

            if (!(metricDefinition is TMetricDefinition castedMetricDefinition))
            {
                throw new ArgumentException($"Could not cast metric definition of type '{metricDefinition.ResourceType}' to {typeof(TMetricDefinition)}. Payload: {JsonConvert.SerializeObject(metricDefinition)}");
            }

            await ScrapeResourceAsync(castedMetricDefinition);
        }

        /// <summary>
        ///     Scrapes the configured resource
        /// </summary>
        /// <param name="metricDefinition">Definition of the metric to scrape</param>
        /// <returns></returns>
        protected abstract Task ScrapeResourceAsync(TMetricDefinition metricDefinition);
    }
}