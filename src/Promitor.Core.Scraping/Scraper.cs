using System;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Interfaces;
using Promitor.Core.Scraping.Prometheus.Interfaces;
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
        private readonly IPrometheusMetricWriter _prometheusMetricWriter;
        private readonly ScraperConfiguration _scraperConfiguration;

        /// <summary>
        ///     Constructor
        /// </summary>
        protected Scraper(ScraperConfiguration scraperConfiguration)
        {
            Guard.NotNull(scraperConfiguration, nameof(scraperConfiguration));

            _logger = scraperConfiguration.Logger;
            _exceptionTracker = scraperConfiguration.ExceptionTracker;
            _scraperConfiguration = scraperConfiguration;
            _prometheusMetricWriter = scraperConfiguration.PrometheusMetricWriter;

            AzureMetadata = scraperConfiguration.AzureMetadata;
            AzureMonitorClient = scraperConfiguration.AzureMonitorClient;
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
                var scrapedMetricResult = await ScrapeResourceAsync(AzureMetadata.SubscriptionId, resourceGroupName, castedMetricDefinition, aggregationType, aggregationInterval.Value);

                _logger.LogInformation("Found value '{MetricValue}' for metric '{MetricName}' with aggregation interval '{AggregationInterval}'", scrapedMetricResult, metricDefinition.Name, aggregationInterval);

                _prometheusMetricWriter.ReportMetric(metricDefinition, scrapedMetricResult);
            }
            catch (ErrorResponseException errorResponseException)
            {
                HandleErrorResponseException(errorResponseException);
            }
            catch (Exception exception)
            {
                _exceptionTracker.Track(exception);
            }
        }

        private void HandleErrorResponseException(ErrorResponseException errorResponseException)
        {
            string reason = string.Empty;

            if (!string.IsNullOrEmpty(errorResponseException.Message))
            {
                reason = errorResponseException.Message;
            }

            if (errorResponseException.Response != null && !string.IsNullOrEmpty(errorResponseException.Response.Content))
            {
                try
                {
                    var definition = new {error = new {code = "", message = ""}};
                    var jsonError = JsonConvert.DeserializeAnonymousType(errorResponseException.Response.Content, definition);

                    if (jsonError != null && jsonError.error != null)
                    {
                        if (!string.IsNullOrEmpty(jsonError.error.message))
                        {
                            reason = $"{jsonError.error.code}: {jsonError.error.message}";
                        }
                        else if (!string.IsNullOrEmpty(jsonError.error.code))
                        {
                            reason = $"{jsonError.error.code}";
                        }
                    }
                }
                catch (Exception)
                {
                    // do nothing. maybe a bad deserialization of json content. Just fallback on outer exception message.
                    _exceptionTracker.Track(errorResponseException);
                }
            }

            _exceptionTracker.Track(new Exception(reason));
        }

        /// <summary>
        ///     Scrapes the configured resource
        /// </summary>
        /// <param name="subscriptionId">Metric subscription Id</param>
        /// <param name="resourceGroupName">Metric Resource Group</param>
        /// <param name="metricDefinition">Definition of the metric to scrape</param>
        /// <param name="aggregationType">Aggregation for the metric to use</param>
        /// <param name="aggregationInterval">Interval that is used to aggregate metrics</param>
        protected abstract Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, TMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval);
    }
}