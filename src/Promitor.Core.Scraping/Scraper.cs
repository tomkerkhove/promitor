using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Metrics.Interfaces;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Interfaces;
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
        private readonly MetricSinkWriter _metricSinkWriter;

        /// <summary>
        ///     Constructor
        /// </summary>
        protected Scraper(ScraperConfiguration scraperConfiguration)
        {
            Guard.NotNull(scraperConfiguration, nameof(scraperConfiguration));

            _metricSinkWriter = scraperConfiguration.MetricSinkWriter;

            Logger = scraperConfiguration.Logger;
            AzureMonitorClient = scraperConfiguration.AzureMonitorClient;
            AzureScrapingSystemMetricsPublisher = scraperConfiguration.AzureScrapingSystemMetricsPublisher;
        }

        /// <summary>
        ///     Client to interact with Azure Monitor
        /// </summary>
        protected AzureMonitorClient AzureMonitorClient { get; }

        /// <summary>
        ///     Collector to send metrics related to the runtime
        /// </summary>
        public IAzureScrapingSystemMetricsPublisher AzureScrapingSystemMetricsPublisher { get; }

        /// <summary>
        ///     Provide logger to scraper
        /// </summary>
        protected ILogger Logger { get; }

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
                    scrapeDefinition.SubscriptionId,
                    scrapeDefinition,
                    castedMetricDefinition,
                    aggregationType,
                    aggregationInterval.Value);

                LogMeasuredMetrics(scrapeDefinition, scrapedMetricResult, aggregationInterval);

                await _metricSinkWriter.ReportMetricAsync(scrapeDefinition.PrometheusMetricDefinition.Name, scrapeDefinition.PrometheusMetricDefinition.Description, scrapedMetricResult);

                await ReportScrapingOutcomeAsync(scrapeDefinition, isSuccessful: true);
            }
            catch (ErrorResponseException errorResponseException)
            {
                HandleErrorResponseException(errorResponseException, scrapeDefinition.PrometheusMetricDefinition.Name);

                await ReportScrapingOutcomeAsync(scrapeDefinition, isSuccessful: false);
            }
            catch (Exception exception)
            {
                Logger.LogCritical(exception, "Failed to scrape resource for metric '{MetricName}'", scrapeDefinition.PrometheusMetricDefinition.Name);

                await ReportScrapingOutcomeAsync(scrapeDefinition, isSuccessful: false);
            }
        }

        private const string ScrapeSuccessfulMetricDescription = "Provides an indication that the scraping of the resource was successful";
        private const string ScrapeErrorMetricDescription = "Provides an indication that the scraping of the resource has failed";

        private async Task ReportScrapingOutcomeAsync(ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, bool isSuccessful)
        {
            // We reset all values, by default
            double successfulMetricValue = 0;
            double unsuccessfulMetricValue = 0;

            // Based on the result, we reflect that in the metric
            if (isSuccessful)
            {
                successfulMetricValue = 1;
            }
            else
            {
                unsuccessfulMetricValue = 1;
            }

            // Enrich with context
            var labels = new Dictionary<string, string>
            {
                {"metric_name", scrapeDefinition.PrometheusMetricDefinition.Name},
                {"resource_group", scrapeDefinition.ResourceGroupName},
                {"resource_name", scrapeDefinition.Resource.ResourceName},
                {"resource_type", scrapeDefinition.Resource.ResourceType.ToString()},
                {"subscription_id", scrapeDefinition.SubscriptionId}
            };

            // Report!
            await AzureScrapingSystemMetricsPublisher.WriteGaugeMeasurementAsync(RuntimeMetricNames.ScrapeSuccessful, ScrapeSuccessfulMetricDescription, successfulMetricValue, labels);
            await AzureScrapingSystemMetricsPublisher.WriteGaugeMeasurementAsync(RuntimeMetricNames.ScrapeError, ScrapeErrorMetricDescription, unsuccessfulMetricValue, labels);
        }

        private void LogMeasuredMetrics(ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, ScrapeResult scrapedMetricResult, TimeSpan? aggregationInterval)
        {
            foreach (var measuredMetric in scrapedMetricResult.MetricValues)
            {
                if (measuredMetric.IsDimensional)
                {
                    Logger.LogInformation("Found value {MetricValue} for metric {MetricName} with dimension {DimensionValue} as part of {DimensionName} dimension with aggregation interval {AggregationInterval}", measuredMetric.Value, scrapeDefinition.PrometheusMetricDefinition.Name, measuredMetric.DimensionValue, measuredMetric.DimensionName, aggregationInterval);
                }
                else
                {
                    Logger.LogInformation("Found value {MetricValue} for metric {MetricName} with aggregation interval {AggregationInterval}", measuredMetric.Value, scrapeDefinition.PrometheusMetricDefinition.Name, aggregationInterval);
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
                    var rawResponse = errorResponseException.Response.Content;
                    var parsedResponse = JToken.Parse(rawResponse);
                    ScrapeError scrapeError;
                    if (string.IsNullOrWhiteSpace(parsedResponse["error"]?.ToString()) == false)
                    {
                        scrapeError = JsonConvert.DeserializeObject<ScrapeError>(parsedResponse["error"].ToString());
                    }
                    else
                    {
                        scrapeError = JsonConvert.DeserializeObject<ScrapeError>(errorResponseException.Response.Content);
                    }
                    reason = ComposeErrorReason(scrapeError);
                }
                catch (Exception)
                {
                    // do nothing. maybe a bad deserialization of json content. Just fallback on outer exception message.
                    Logger.LogCritical(errorResponseException, "Failed to scrape resource for metric '{MetricName}'", metricName);
                }
            }

            Logger.LogCritical(reason);
        }

        private static string ComposeErrorReason(ScrapeError scrapeError)
        {
            string reason = string.Empty;
            if (!string.IsNullOrEmpty(scrapeError.Message))
            {
                reason = $"{scrapeError.Code}: {scrapeError.Message}";
            }
            else if (!string.IsNullOrEmpty(scrapeError.Code))
            {
                reason = $"{scrapeError.Code}";
            }

            return reason;
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