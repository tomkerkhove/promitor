using System.Collections.Generic;
using Promitor.Core.Contracts;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    public class MetricDefinition
    {
        public MetricDefinition()
        {
        }

        public MetricDefinition(PrometheusMetricDefinition prometheusMetricDefinition,
            Scraping scraping,
            AzureMetricConfiguration azureMetricConfiguration,
            LogAnalyticsConfiguration logAnalyticsConfiguration,
            ResourceType resourceType,
            List<AzureResourceDefinition> resources)
        {
            AzureMetricConfiguration = azureMetricConfiguration;
            LogAnalyticsConfiguration = logAnalyticsConfiguration;
            PrometheusMetricDefinition = prometheusMetricDefinition;
            Scraping = scraping;
            ResourceType = resourceType;
            Resources = resources;
        }

        /// <summary>
        ///     Configuration about the Azure Monitor metric to scrape
        /// </summary>
        public AzureMetricConfiguration AzureMetricConfiguration { get; set; }

        /// <summary>
        ///     Configuration about the Azure Log Analytics to scrape
        /// </summary>
        public LogAnalyticsConfiguration LogAnalyticsConfiguration { get; set; }

        /// <summary>
        /// The details of the prometheus metric that will be created.
        /// </summary>
        public PrometheusMetricDefinition PrometheusMetricDefinition { get; set; }

        /// <summary>
        /// Gets or sets the scraping model.
        /// </summary>
        public Scraping Scraping { get; set; } = new Scraping();

        /// <summary>
        /// Type of resource that is configured
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the list of resource collections to discover resources with.
        /// </summary>
        public List<AzureResourceDiscoveryGroup> ResourceDiscoveryGroups { get; set; }

        /// <summary>
        /// Gets or sets the list of resources to scrape.
        /// </summary>
        public List<AzureResourceDefinition> Resources { get; set; }

        /// <summary>
        /// Creates a <see cref="ScrapeDefinition{TResourceDefinition}"/> object for the specified resource.
        /// </summary>
        /// <param name="resource">The resource to scrape.</param>
        /// <param name="azureMetadata">The Azure global metadata.</param>
        /// <returns>The scrape definition.</returns>
        public ScrapeDefinition<IAzureResourceDefinition> CreateScrapeDefinition(IAzureResourceDefinition resource, AzureMetadata azureMetadata)
        {
            var subscriptionId = string.IsNullOrEmpty(resource.SubscriptionId) ? azureMetadata.SubscriptionId : resource.SubscriptionId;
            var resourceGroupName = string.IsNullOrEmpty(resource.ResourceGroupName) ? azureMetadata.ResourceGroupName : resource.ResourceGroupName;

            var output =  new ScrapeDefinition<IAzureResourceDefinition>(
                AzureMetricConfiguration,
                LogAnalyticsConfiguration,
                PrometheusMetricDefinition,
                Scraping,
                resource,
                subscriptionId,
                resourceGroupName);

            return output;
        }
    }
}
