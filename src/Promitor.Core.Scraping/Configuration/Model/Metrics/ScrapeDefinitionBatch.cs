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
        /// <param name="prometheusMetricDefinition">The details of the prometheus metric that will be created.</param>
        /// <param name="scraping">The scraping model.</param>
        /// <param name="resource">The resource to scrape.</param>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">
        /// The name of the resource group containing the resource to scrape. This should contain the global
        /// resource group name if none is overridden at the resource level.
        /// </param>
        public ScrapeDefinitionBatchProperties(
            AzureMetricConfiguration azureMetricConfiguration,
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
        /// The details of the prometheus metric that will be created.
        /// </summary>
        public PrometheusMetricDefinition PrometheusMetricDefinition { get; }

        /// <summary>
        /// The scraping model.
        /// </summary>
        public Scraping Scraping { get; }

        /// <summary>
        /// The Azure subscription to get the metric from. This should be used instead of using
        /// the SubscriptionId from <see cref="Resource"/> because this property will contain
        /// the global subscription id if none is overridden at the resource level.
        /// </summary>
        public string SubscriptionId { get; }

        /// <summary>
        /// The Azure resource type shared by all scrape definitions in the batch
        /// </summary>
        public ResourceType ResourceType { get; }

        public TimeSpan AggregationInterval{ get; }

        public TimeSpan? GetAggregationInterval()
        {
            return AzureMetricConfiguration?.Aggregation?.Interval;
        }

        public override int GetHashCode()
        {
            return this.BuildBatchHashKey().GetHashCode(); 
        }

        /// <summary>
        /// Builds a namespaced string key to satisfy batch restrictions, in the format of
        /// <AzureMetricAndDimensions>_<SubscriptionId>_<ResourceType>_<AggregationInterval> 
        /// </summary>
        private string BuildBatchHashKey()
        {
            return string.Join("_",  [List.ofAzureMetricConfiguration.ToUniqueStringRepresentation(), SubscriptionId, ResourceType.ToString(), AggregationInterval.ToString]);
        }

        /// <summary>
        /// Equality comparison override in case of hash collision
        /// </summary>
        public bool Equals(ScrapeDefinitionBatchProperties obj)
        {
            if (obj == null || !(obj is ScrapeDefinitionBatchProperties))
                return false;

            ScrapeDefinitionBatchProperties other = (ScrapeDefinitionBatchProperties)obj;
            return this.ResourceType == other.ResourceType && this.AzureMetricConfiguration.ToUniqueStringRepresentation() == other.ToUniqueStringRepresentation() && this.SubscriptionId == other.SubscriptionId && this.AggregationInterval.Equals(other.AggregationDeserializer);
        }

    }
}