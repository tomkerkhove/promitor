using System;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Prometheus.Client;
using Promitor.Core.Infrastructure;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Interfaces;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Integrations.AzureMonitor;
using MetricDefinition = Promitor.Core.Scraping.Configuration.Model.Metrics.MetricDefinition;
// ReSharper disable All

namespace Promitor.Core.Scraping
{
    /// <summary>
    ///     Azure Monitor Scraper
    /// </summary>
    /// <typeparam name="TMetricDefinition">Type of metric definition that is being used</typeparam>
    public abstract class Scraper<TMetricDefinition> : IScraper<MetricDefinition>
      where TMetricDefinition : MetricDefinition, new()
    {
        private readonly IExceptionTracker _exceptionTracker;
        private readonly ILogger _logger;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="azureMetadata">Metadata concerning the Azure resources</param>
        /// <param name="azureMonitorClient">Client to communicate with Azure Monitor</param>
        /// <param name="logger">General logger</param>
        /// <param name="exceptionTracker">Exception tracker</param>
        protected Scraper(AzureMetadata azureMetadata, AzureMonitorClient azureMonitorClient, ILogger logger,
            IExceptionTracker exceptionTracker)
        {
            Guard.NotNull(exceptionTracker, nameof(exceptionTracker));
            Guard.NotNull(azureMetadata, nameof(azureMetadata));
            Guard.NotNull(azureMonitorClient, nameof(azureMonitorClient));

            _logger = logger;
            _exceptionTracker = exceptionTracker;

            this.AzureMetadata = azureMetadata;
            this.AzureMonitorClient = azureMonitorClient;
        }

        /// <summary>
        ///     Metadata concerning the Azure resources
        /// </summary>
        protected AzureMetadata AzureMetadata { get; }

        /// <summary>
        ///     Default configuration for metrics
        /// </summary>
        protected MetricDefaults MetricDefaults { get; }

        /// <summary>
        ///     Client to interact with Azure Monitor
        /// </summary>
        protected AzureMonitorClient AzureMonitorClient { get; }

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

                var aggregationInterval = metricDefinition.AzureMetricConfiguration.Aggregation.Interval;
                var aggregationType = metricDefinition.AzureMetricConfiguration.Aggregation.Type;
                var resourceGroupName = string.IsNullOrEmpty(metricDefinition.ResourceGroupName) ? AzureMetadata.ResourceGroupName : metricDefinition.ResourceGroupName;
                var foundMetricValue = await ScrapeResourceAsync(AzureMetadata.SubscriptionId, resourceGroupName, castedMetricDefinition, aggregationType, aggregationInterval.Value);

                _logger.LogInformation("Found value '{MetricValue}' for metric '{MetricName}' with aggregation interval '{AggregationInterval}'", foundMetricValue, metricDefinition.Name, aggregationInterval);

                var metricsTimestampFeatureFlag = FeatureFlag.IsActive("METRICSTIMESTAMP", defaultFlagState: true);

                var gauge = Metrics.CreateGauge(metricDefinition.Name, metricDefinition.Description, includeTimestamp: metricsTimestampFeatureFlag);
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
        /// <param name="subscriptionId">Metric subscription Id</param>
        /// <param name="resourceGroupName">Metric Resource Group</param>
        /// <param name="metricDefinition">Definition of the metric to scrape</param>
        /// <param name="aggregationType">Aggregation for the metric to use</param>
        /// <param name="aggregationInterval">Interval that is used to aggregate metrics</param>
        /// 
        protected abstract Task<double> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, TMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval);
    }
}
