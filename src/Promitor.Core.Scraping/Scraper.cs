using System;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Interfaces;
using Promitor.Core.Scraping.Prometheus.Interfaces;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Core.Scraping
{
    /// <summary>
    ///     A generic scraper
    /// </summary>
    /// <typeparam name="TResourceDefinition">Type of metric definition that is being used</typeparam>
    public abstract class Scraper<TResourceDefinition> : IScraper<IAzureResourceDefinition>
      where TResourceDefinition : class, IAzureResourceDefinition
    {
        private readonly ILogger _logger;
        private readonly IPrometheusMetricWriter _prometheusMetricWriter;

        /// <summary>
        ///     Constructor
        /// </summary>
        protected Scraper(ScraperConfiguration scraperConfiguration)
        {
            Guard.NotNull(scraperConfiguration, nameof(scraperConfiguration));

            _logger = scraperConfiguration.Logger;
            _prometheusMetricWriter = scraperConfiguration.PrometheusMetricWriter;

            AzureMetadata = scraperConfiguration.AzureMetadata;
            AzureMonitorClient = scraperConfiguration.AzureMonitorClient;
        }

        /// <summary>
        ///     Metadata concerning the Azure resources
        /// </summary>
        protected AzureMetadata AzureMetadata { get; }

        /// <summary>
        ///     Client to interact with Azure Monitor
        /// </summary>
        protected AzureMonitorClient AzureMonitorClient { get; }

        public async Task ScrapeAsync(ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition)
        {
            if (scrapeDefinition == null)
            {
                throw new ArgumentNullException(nameof(scrapeDefinition));
            }

            var aggregationInterval = scrapeDefinition.AzureMetricConfiguration?.Aggregation?.Interval;
            if (aggregationInterval == null)
            {
                throw new ArgumentNullException(nameof(scrapeDefinition));
            }

            try
            {
                var castedMetricDefinition = scrapeDefinition.Resource as TResourceDefinition;
                if (castedMetricDefinition == null)
                {
                    throw new ArgumentException($"Could not cast metric definition of type {scrapeDefinition.Resource.ResourceType} to {typeof(TResourceDefinition)}. Payload: {JsonConvert.SerializeObject(scrapeDefinition)}");
                }

                var aggregationType = scrapeDefinition.AzureMetricConfiguration.Aggregation.Type;
                var scrapedMetricResult = await ScrapeResourceAsync(
                    AzureMetadata.SubscriptionId,
                    scrapeDefinition,
                    castedMetricDefinition,
                    aggregationType,
                    aggregationInterval.Value);

                LogMeasuredMetrics(scrapeDefinition, scrapedMetricResult, aggregationInterval);

                _prometheusMetricWriter.ReportMetric(scrapeDefinition.PrometheusMetricDefinition, scrapedMetricResult);
            }
            catch (ErrorResponseException errorResponseException)
            {
                HandleErrorResponseException(errorResponseException, scrapeDefinition.PrometheusMetricDefinition.Name);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Failed to scrape resource for metric '{MetricName}'", scrapeDefinition.PrometheusMetricDefinition.Name);
            }
        }

        private void LogMeasuredMetrics(ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, ScrapeResult scrapedMetricResult, TimeSpan? aggregationInterval)
        {
            foreach (var measuredMetric in scrapedMetricResult.MetricValues)
            {
                if (measuredMetric.IsDimensional)
                {
                    _logger.LogInformation("Found value {MetricValue} for metric {MetricName} with dimension {DimensionValue} as part of {DimensionName} dimension with aggregation interval {AggregationInterval}", measuredMetric.Value, scrapeDefinition.PrometheusMetricDefinition.Name, measuredMetric.DimensionValue, measuredMetric.DimensionName, aggregationInterval);
                }
                else
                {
                    _logger.LogInformation("Found value {MetricValue} for metric {MetricName} with aggregation interval {AggregationInterval}", measuredMetric.Value, scrapeDefinition.PrometheusMetricDefinition.Name, aggregationInterval);
                }
            }
        }

        private void HandleErrorResponseException(ErrorResponseException errorResponseException, string metricName)
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
                    var definition = new { error = new { code = "", message = "" } };
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
                    _logger.LogCritical(errorResponseException, "Failed to scrape resource for metric '{MetricName}'", metricName);
                }
            }

            _logger.LogCritical(reason);
        }

        /// <summary>
        ///     Scrapes the configured resource
        /// </summary>
        /// <param name="subscriptionId">Metric subscription Id</param>
        /// <param name="scrapeDefinition">Contains all the information needed to scrape the resource.</param>
        /// <param name="resourceDefinition">Contains the resource cast to the specific resource type.</param>
        /// <param name="aggregationType">Aggregation for the metric to use</param>
        /// <param name="aggregationInterval">Interval that is used to aggregate metrics</param>
        protected abstract Task<ScrapeResult> ScrapeResourceAsync(
            string subscriptionId,
            ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition,
            TResourceDefinition resourceDefinition,
            AggregationType aggregationType,
            TimeSpan aggregationInterval);

        /// <summary>
        /// Builds the URI of the resource to scrape
        /// </summary>
        /// <param name="subscriptionId">Subscription id in which the resource lives</param>
        /// <param name="scrapeDefinition">Contains all the information needed to scrape the resource.</param>
        /// <param name="resource">Contains the resource cast to the specific resource type.</param>
        /// <returns>Uri of Azure resource</returns>
        protected abstract string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, TResourceDefinition resource);
    }
}