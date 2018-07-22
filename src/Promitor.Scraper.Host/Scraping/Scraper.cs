using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Configuration.Model.Metrics;
using Promitor.Scraper.Host.Model.Configuration;
using Promitor.Scraper.Host.Scraping.Interfaces;
using Prometheus.Client;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Scraper.Host.Scraping
{
    /// <summary>
    ///     Azure Monitor Scrape
    /// </summary>
    /// <typeparam name="TMetricDefinition">Type of metric definition that is being used</typeparam>
    public abstract class Scraper<TMetricDefinition> : IScraper<MetricDefinition>
        where TMetricDefinition : MetricDefinition, new()
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="azureMetadata">Metadata concerning the Azure resources</param>
        /// <param name="azureCredentials">Credentials used to authenticate to Microsoft Azure</param>
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

            var azureMonitorClient = new AzureMonitorClient(AzureMetadata.TenantId, AzureMetadata.SubscriptionId, AzureCredentials.ApplicationId, AzureCredentials.Secret);
            var foundMetricValue = await ScrapeResourceAsync(azureMonitorClient, castedMetricDefinition);

            var gauge = Metrics.CreateGauge(metricDefinition.Name, metricDefinition.Description);
            gauge.Set(foundMetricValue);
        }

        /// <summary>
        ///     Scrapes the configured resource
        /// </summary>
        /// <param name="azureMonitorClient">Client to query Azure Monitor</param>
        /// <param name="metricDefinition">Definition of the metric to scrape</param>
        protected abstract Task<double> ScrapeResourceAsync(AzureMonitorClient azureMonitorClient, TMetricDefinition metricDefinition);
    }
}