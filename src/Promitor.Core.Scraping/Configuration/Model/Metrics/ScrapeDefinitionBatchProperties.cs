using System;
using System.Collections.Generic;
using GuardNet;
using Promitor.Core.Contracts;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    /// <summary>
    /// Defines properties of a batch of scrape definitions 
    /// </summary>
    public class ScrapeDefinitionBatchProperties : IEquatable<ScrapeDefinitionBatchProperties>
    { 
        /// <param name="azureMetricConfiguration">Configuration about the Azure Monitor metric to scrape</param>
        /// <param name="logAnalyticsConfiguration">Configuration about the LogAnalytics resource to scrape</param>
        /// <param name="prometheusMetricDefinition">The details of the prometheus metric that will be created.</param>
        /// <param name="scraping">The scraping model.</param>
        /// <param name="resourceType">Resource type of the batch</param>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        public ScrapeDefinitionBatchProperties(
            AzureMetricConfiguration azureMetricConfiguration,
            LogAnalyticsConfiguration logAnalyticsConfiguration,
            PrometheusMetricDefinition prometheusMetricDefinition,
            ResourceType resourceType,
            Scraping scraping,
            string subscriptionId)
        {
            Guard.NotNull(azureMetricConfiguration, nameof(azureMetricConfiguration));
            Guard.NotNull(prometheusMetricDefinition, nameof(prometheusMetricDefinition));
            Guard.NotNull(scraping, nameof(scraping));
            Guard.NotNull(subscriptionId, nameof(subscriptionId));

            AzureMetricConfiguration = azureMetricConfiguration;
            LogAnalyticsConfiguration = logAnalyticsConfiguration;
            PrometheusMetricDefinition = prometheusMetricDefinition;
            Scraping = scraping;
            SubscriptionId = subscriptionId;
            ResourceType = resourceType;
       }

        /// <summary>
        /// Configuration about the Azure Monitor metric to scrape
        /// </summary>
        public AzureMetricConfiguration AzureMetricConfiguration { get; }

        /// <summary>
        /// Configuration about the Azure Monitor log analytics resource to scrape
        /// </summary>
        public LogAnalyticsConfiguration LogAnalyticsConfiguration { get; }

        /// <summary>
        /// The details of the prometheus metric that will be created.
        /// </summary>
        public PrometheusMetricDefinition PrometheusMetricDefinition { get; }

        /// <summary>
        /// The scraping model.
        /// </summary>
        public Scraping Scraping { get; }

        /// <summary>
        /// The Azure subscription to get the metric from.
        /// </summary>
        public string SubscriptionId { get; }

        /// <summary>
        /// The Azure resource type shared by all scrape definitions in the batch
        /// </summary>
        public ResourceType ResourceType { get; }

        public TimeSpan? GetAggregationInterval()
        {
            if (ResourceType == ResourceType.LogAnalytics)
            {
                return LogAnalyticsConfiguration?.Aggregation?.Interval;
            }
            return AzureMetricConfiguration?.Aggregation?.Interval;
        }

        public override int GetHashCode()
        {
            return this.BuildBatchHashKey().GetHashCode(); 
        }

        /// <summary>
        /// Builds a namespaced string key to satisfy batch restrictions, in the format of
        /// (AzureMetricAndDimensionsAndFilter)_(SubscriptionId)_(ResourceType)_(AggregationInterval>)
        /// </summary>
        public string BuildBatchHashKey()
        {
            return string.Join("_", new List<string> {AzureMetricConfiguration.ToUniqueStringRepresentation(), SubscriptionId, ResourceType.ToString(), GetAggregationInterval().ToString()});
        }

        /// <summary>
        /// Equality comparison override in case of hash collision
        /// </summary>
        public bool Equals(ScrapeDefinitionBatchProperties other)
        {
            if (other is null) {
                return false;
            }
            
            return ResourceType == other.ResourceType && AzureMetricConfiguration.ToUniqueStringRepresentation() == other.AzureMetricConfiguration.ToUniqueStringRepresentation() && SubscriptionId == other.SubscriptionId && GetAggregationInterval().Equals(other.GetAggregationInterval());
        }
    }
}