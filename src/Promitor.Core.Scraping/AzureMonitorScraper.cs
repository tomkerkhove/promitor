using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Promitor.Core.Contracts;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Integrations.AzureMonitor.Exceptions;

namespace Promitor.Core.Scraping
{
    /// <summary>
    ///     Azure Monitor Scraper
    /// </summary>
    /// <typeparam name="TResourceDefinition">Type of metric definition that is being used</typeparam>
    public abstract class AzureMonitorScraper<TResourceDefinition> : Scraper<TResourceDefinition>
        where TResourceDefinition : class, IAzureResourceDefinition
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        protected AzureMonitorScraper(ScraperConfiguration scraperConfiguration) :
            base(scraperConfiguration)
        {
        }

        /// <inheritdoc />
        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, TResourceDefinition resourceDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            Guard.NotNull(scrapeDefinition, nameof(scrapeDefinition));
            Guard.NotNull(scrapeDefinition.AzureMetricConfiguration, nameof(scrapeDefinition.AzureMetricConfiguration));

            var metricName = scrapeDefinition.AzureMetricConfiguration.MetricName;

            // Compose URI of the resource to measure
            var resourceUri = BuildResourceUri(subscriptionId, scrapeDefinition, resourceDefinition);

            // Determine the metric filter to use, if any
            var metricFilter = DetermineMetricFilter(metricName, resourceDefinition);

            // Determine the metric limit to use, if any
            var metricLimit = DetermineMetricLimit(scrapeDefinition);

            // Determine the metric dimension to use, if any
            var dimensionNames = DetermineMetricDimensions(metricName, resourceDefinition, scrapeDefinition.AzureMetricConfiguration);

            var measuredMetrics = new List<MeasuredMetric>();
            try
            {
                // Query Azure Monitor for metrics
                measuredMetrics = await AzureMonitorClient.QueryMetricAsync(metricName, dimensionNames, aggregationType, aggregationInterval, resourceUri, metricFilter, metricLimit);
            }
            catch (MetricInformationNotFoundException metricsNotFoundException)
            {
                Logger.LogWarning("No metric information found for metric {MetricName} with dimensions {MetricDimensions}. Details: {Details}", metricsNotFoundException.Name, metricsNotFoundException.Dimensions, metricsNotFoundException.Details);
                
                var measuredMetric = dimensionNames.Any() 
                            ? MeasuredMetric.CreateForDimensions(dimensionNames) 
                            : MeasuredMetric.CreateWithoutDimensions(null);
                measuredMetrics.Add(measuredMetric);
            }

            // Provide more metric labels, if we need to
            var metricLabels = DetermineMetricLabels(resourceDefinition);

            // Enrich measured metrics, in case we need to
            var finalMetricValues = EnrichMeasuredMetrics(resourceDefinition, dimensionNames, measuredMetrics);

            // We're done!
            return new ScrapeResult(subscriptionId, scrapeDefinition.ResourceGroupName, resourceDefinition.ResourceName, resourceUri, finalMetricValues, metricLabels);
        }

        private int? DetermineMetricLimit(ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition)
        {
            return scrapeDefinition.AzureMetricConfiguration.Limit;
        }

        /// <summary>
        ///     Enrich measured metrics
        /// </summary>
        /// <remarks>
        ///     This should only be used by scrapers who need to provide more information or need to tweak the reported
        ///     metrics to align with others
        /// </remarks>
        /// <param name="resourceDefinition">Contains the resource cast to the specific resource type.</param>
        /// <param name="dimensionNames">List of names of the specified dimensions provided by the scraper.</param>
        /// <param name="metricValues">Measured metric values that were found</param>
        /// <returns></returns>
        protected virtual List<MeasuredMetric> EnrichMeasuredMetrics(TResourceDefinition resourceDefinition, List<string> dimensionNames, List<MeasuredMetric> metricValues)
        {
            return metricValues;
        }

        /// <summary>
        ///     Determines the metric filter to use
        /// </summary>
        /// <param name="metricName">Name of the metric being queried</param>
        /// <param name="resourceDefinition">Contains the resource cast to the specific resource type.</param>
        protected virtual string DetermineMetricFilter(string metricName, TResourceDefinition resourceDefinition)
        {
            return null;
        }

        /// <summary>
        ///     Determines the dimension for a metric to use
        /// </summary>
        /// <param name="metricName">Name of the metric being queried</param>
        /// <param name="resourceDefinition">Contains the resource cast to the specific resource type.</param>
        /// <param name="configuration"></param>
        protected virtual List<string> DetermineMetricDimensions(string metricName, TResourceDefinition resourceDefinition, AzureMetricConfiguration configuration)
        {
            if (configuration.Dimension != null)
            {
                Logger.LogWarning("AzureMetricConfiguration property 'dimension' has been deprecated and will be removed in a future update. Please use 'dimensions' instead.");
                return string.IsNullOrWhiteSpace(configuration.Dimension.Name) ? new List<string>() : new List<string>{ configuration.Dimension.Name };
            }

            return configuration.Dimensions?.Select(dimension => dimension.Name).Where(dimensionName => !string.IsNullOrWhiteSpace(dimensionName)).Distinct().ToList();
        }

        /// <summary>
        ///     Determines the metric labels to include in the reported metric
        /// </summary>
        /// <param name="resourceDefinition">Contains the resource cast to the specific resource type.</param>
        protected virtual Dictionary<string, string> DetermineMetricLabels(TResourceDefinition resourceDefinition)
        {
            return new Dictionary<string, string>();
        }
    }
}