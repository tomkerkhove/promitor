using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Contracts;
using Promitor.Core.Extensions;
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
        ///     A cache to store resource definitions. Used to hydrate resource info from resource ID, when processing batch query results
        /// </summary>
        private readonly ConcurrentDictionary<string, IAzureResourceDefinition> _resourceDefinitions; // using a dictionary for now since IMemoryCache involves layers of injection

        /// <summary>
        ///     Constructor
        /// </summary>
        protected AzureMonitorScraper(ScraperConfiguration scraperConfiguration) :
            base(scraperConfiguration)
        {
            _resourceDefinitions = new ConcurrentDictionary<string, IAzureResourceDefinition>();
        }

        /// <inheritdoc />
        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, TResourceDefinition resourceDefinition, PromitorMetricAggregationType aggregationType, TimeSpan aggregationInterval)
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

        protected override async Task<List<ScrapeResult>> BatchScrapeResourceAsync(string subscriptionId, BatchScrapeDefinition<IAzureResourceDefinition> batchScrapeDefinition, PromitorMetricAggregationType aggregationType, TimeSpan aggregationInterval)
        {
            Guard.NotNull(batchScrapeDefinition, nameof(batchScrapeDefinition));
            Guard.NotLessThan(batchScrapeDefinition.ScrapeDefinitions.Count(), 1, nameof(batchScrapeDefinition));
            Guard.NotNull(batchScrapeDefinition.ScrapeDefinitionBatchProperties.AzureMetricConfiguration, nameof(batchScrapeDefinition.ScrapeDefinitionBatchProperties.AzureMetricConfiguration));

            var metricName = batchScrapeDefinition.ScrapeDefinitionBatchProperties.AzureMetricConfiguration.MetricName;

            // Build list of resource URIs based on definitions in the batch 
            var resourceUriList = new List<string>();
            foreach (ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition in batchScrapeDefinition.ScrapeDefinitions)
            {
                var resourceUri = BuildResourceUri(subscriptionId, scrapeDefinition, (TResourceDefinition) scrapeDefinition.Resource);
                resourceUriList.Add(resourceUri);
                // cache resource info 
                if (!_resourceDefinitions.ContainsKey(resourceUri))
                {
                    var resourceDefinitionToCache = new AzureResourceDefinition
                    (
                        resourceType: scrapeDefinition.Resource.ResourceType, 
                        resourceGroupName:  scrapeDefinition.ResourceGroupName, 
                        subscriptionId: scrapeDefinition.SubscriptionId, 
                        resourceName: scrapeDefinition.Resource.ResourceName
                    ); // the resource definition attached is missing some attributes, filling them in here
                    Logger.LogWarning("Caching resource group {Group} for {ResourceId}",  resourceDefinitionToCache.ResourceGroupName, resourceUri);
                    _resourceDefinitions.TryAdd(resourceUri, resourceDefinitionToCache);
                }
            }

            var metricFilter = DetermineMetricFilter(metricName, (TResourceDefinition) batchScrapeDefinition.ScrapeDefinitions[0].Resource);
            var metricLimit = batchScrapeDefinition.ScrapeDefinitionBatchProperties.AzureMetricConfiguration.Limit;
            var dimensionNames = DetermineMetricDimensions(metricName, (TResourceDefinition) batchScrapeDefinition.ScrapeDefinitions[0].Resource, batchScrapeDefinition.ScrapeDefinitionBatchProperties.AzureMetricConfiguration); // TODO: resource definition doesn't seem to be used, can we remove it from function signature?  

            var resourceIdTaggedMeasuredMetrics = new List<ResourceAssociatedMeasuredMetric>();
            try
            {
                // Query Azure Monitor for metrics
                Logger.LogWarning("Querying Azure Monitor for metric {MetricName} with batch size {BatchSize}", metricName, resourceUriList.Count);
                resourceIdTaggedMeasuredMetrics = await AzureMonitorClient.BatchQueryMetricAsync(metricName, dimensionNames, aggregationType, aggregationInterval, resourceUriList, metricFilter, metricLimit);
            }
            catch (MetricInformationNotFoundException metricsNotFoundException)
            {
                Logger.LogWarning("No metric information found for metric {MetricName} with dimensions {MetricDimensions}. Details: {Details}", metricsNotFoundException.Name, metricsNotFoundException.Dimensions, metricsNotFoundException.Details);
                
                var measuredMetric = dimensionNames.Any() 
                            ? MeasuredMetric.CreateForDimensions(dimensionNames) 
                            : MeasuredMetric.CreateWithoutDimensions(null);
                resourceIdTaggedMeasuredMetrics.Add(measuredMetric.WithResourceIdAssociation(null));
            }

            var scrapeResults = new List<ScrapeResult>();
            // group based on resource, then to enrichment per group
            var groupedMeasuredMetrics = resourceIdTaggedMeasuredMetrics.GroupBy(measureMetric => measureMetric.ResourceId);
            foreach (IGrouping<string, ResourceAssociatedMeasuredMetric> resourceMetricsGroup in groupedMeasuredMetrics) 
            {
                var resourceId = resourceMetricsGroup.Key; 
                _resourceDefinitions.TryGetValue(resourceId, out IAzureResourceDefinition resourceDefinition);
                var metricLabels = DetermineMetricLabels((TResourceDefinition) batchScrapeDefinition.ScrapeDefinitions[0].Resource);
                var finalMetricValues = EnrichMeasuredMetrics((TResourceDefinition) batchScrapeDefinition.ScrapeDefinitions[0].Resource, dimensionNames, resourceMetricsGroup.ToImmutableList());
                scrapeResults.Add(new ScrapeResult(subscriptionId, resourceDefinition.ResourceGroupName, resourceDefinition.ResourceName, resourceId, finalMetricValues, metricLabels));
                Logger.LogWarning("Processed {MetricsCount} measured metrics for Metric {MetricName} and resource {ResourceName}", finalMetricValues.Count, metricName, resourceId);
            }

            return scrapeResults;
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
        protected virtual List<MeasuredMetric> EnrichMeasuredMetrics(TResourceDefinition resourceDefinition, List<string> dimensionNames, IReadOnlyList<MeasuredMetric> metricValues)
        {
            return metricValues.ToList();
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