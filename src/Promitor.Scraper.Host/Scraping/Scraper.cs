using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prometheus.Client;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Integrations.AzureMonitor;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Configuration.Model.Metrics;
using Promitor.Scraper.Host.Scraping.Interfaces;

namespace Promitor.Scraper.Host.Scraping
{
    /// <summary>
    ///     Azure Monitor Scrape
    /// </summary>
    /// <typeparam name="TMetricDefinition">Type of metric definition that is being used</typeparam>
    public abstract class Scraper<TMetricDefinition> : IScraper<MetricDefinition>
        where TMetricDefinition : MetricDefinition, new()
    {
        private readonly IExceptionTracker _exceptionTracker;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="azureMetadata">Metadata concerning the Azure resources</param>
        /// <param name="azureCredentials">Credentials used to authenticate to Microsoft Azure</param>
        /// <param name="exceptionTracker">Exception tracker</param>
        protected Scraper(AzureMetadata azureMetadata, AzureCredentials azureCredentials, IExceptionTracker exceptionTracker)
        {
            Guard.Guard.NotNull(exceptionTracker, nameof(exceptionTracker));

            _exceptionTracker = exceptionTracker;

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
            try
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
            catch (Exception exception)
            {
                _exceptionTracker.Track(exception);
            }
        }

        /// <summary>
        ///     Scrapes the configured resource
        /// </summary>
        /// <param name="azureMonitorClient">Client to query Azure Monitor</param>
        /// <param name="metricDefinition">Definition of the metric to scrape</param>
        protected abstract Task<double> ScrapeResourceAsync(AzureMonitorClient azureMonitorClient, TMetricDefinition metricDefinition);
    }
}