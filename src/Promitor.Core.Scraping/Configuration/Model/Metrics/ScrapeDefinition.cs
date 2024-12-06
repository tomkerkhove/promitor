using System;
using GuardNet;
using Promitor.Core.Contracts;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    /// <summary>
    /// Defines an individual Azure resource to be scraped.
    /// </summary>
    public class ScrapeDefinition<TResourceDefinition> where TResourceDefinition : class, IAzureResourceDefinition
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ScrapeDefinition{TResourceDefinition}"/> class.
        /// </summary>
        /// <param name="azureMetricConfiguration">Configuration about the Azure Monitor metric to scrape</param>
        /// <param name="logAnalyticsConfiguration">Configuration about the Azure LogAnalytics to scrape</param>
        /// <param name="prometheusMetricDefinition">The details of the prometheus metric that will be created.</param>
        /// <param name="scraping">The scraping model.</param>
        /// <param name="resource">The resource to scrape.</param>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">
        /// The name of the resource group containing the resource to scrape. This should contain the global
        /// resource group name if none is overridden at the resource level.
        /// </param>
        public ScrapeDefinition(
            AzureMetricConfiguration azureMetricConfiguration,
            LogAnalyticsConfiguration logAnalyticsConfiguration,
            PrometheusMetricDefinition prometheusMetricDefinition,
            Scraping scraping,
            TResourceDefinition resource,
            string subscriptionId,
            string resourceGroupName)
        {
            Guard.NotNull(azureMetricConfiguration, nameof(azureMetricConfiguration));
            Guard.NotNull(prometheusMetricDefinition, nameof(prometheusMetricDefinition));
            Guard.NotNull(scraping, nameof(scraping));
            Guard.NotNull(resource, nameof(resource));
            Guard.NotNull(subscriptionId, nameof(subscriptionId));
            Guard.NotNull(resourceGroupName, nameof(resourceGroupName));

            AzureMetricConfiguration = azureMetricConfiguration;
            LogAnalyticsConfiguration = logAnalyticsConfiguration;
            PrometheusMetricDefinition = prometheusMetricDefinition;
            Scraping = scraping;
            Resource = resource;
            SubscriptionId = subscriptionId;
            ResourceGroupName = resourceGroupName;
        }

        /// <summary>
        /// Configuration about the Azure Monitor metric to scrape
        /// </summary>
        public AzureMetricConfiguration AzureMetricConfiguration { get; }

        /// <summary>
        /// Configuration about the Azure LogAnalytics to scrape
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
        /// The resource to scrape.
        /// </summary>
        public TResourceDefinition Resource { get; }

        /// <summary>
        /// The Azure subscription to get the metric from. This should be used instead of using
        /// the SubscriptionId from <see cref="Resource"/> because this property will contain
        /// the global subscription id if none is overridden at the resource level.
        /// </summary>
        public string SubscriptionId { get; }

        /// <summary>
        /// The Azure resource group to get the metric from. This should be used instead of using
        /// the ResourceGroupName from <see cref="Resource"/> because this property will contain
        /// the global resource group name if none is overridden at the resource level.
        /// </summary>
        public string ResourceGroupName { get; }

        public TimeSpan? GetAggregationInterval()
        {
            if (Resource.ResourceType == ResourceType.LogAnalytics)
            {
                return LogAnalyticsConfiguration?.Aggregation?.Interval;
            }
            return AzureMetricConfiguration?.Aggregation?.Interval;
        }
    }
}
