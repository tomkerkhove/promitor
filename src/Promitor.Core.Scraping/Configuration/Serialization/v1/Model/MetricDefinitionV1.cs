using System.Collections.Generic;
using Promitor.Core.Contracts;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    /// <summary>
    /// Contains the definition for a prometheus metric, along with the resources
    /// that should be scraped to supply the metric.
    /// </summary>
    public class MetricDefinitionV1
    {
        /// <summary>
        /// The name of the prometheus metric.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the prometheus metric.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The type of resources that are scraped to populate this metric.
        /// </summary>
        public ResourceType? ResourceType { get; set; }

        /// <summary>
        /// Any prometheus labels that should be added to the metric.
        /// </summary>
        public Dictionary<string, string> Labels { get; set; }

        /// <summary>
        /// Contains the configuration used when querying Azure metrics.
        /// </summary>
        public AzureMetricConfigurationV1 AzureMetricConfiguration { get; set; }

        /// <summary>
        /// Contains the configuration used when querying Log Analytics
        /// </summary>
        public LogAnalyticsConfigurationV1 LogAnalyticsConfiguration { get; set; }

        /// <summary>
        /// Allows a custom scraping schedule to be specified for the metric.
        /// </summary>
        public ScrapingV1 Scraping { get; set; }

        /// <summary>
        /// The resource collections to be scraped.
        /// </summary>
        public IReadOnlyCollection<AzureResourceDiscoveryGroupDefinitionV1> ResourceDiscoveryGroups { get; set; }

        /// <summary>
        /// The resources to be scraped.
        /// </summary>
        public IReadOnlyCollection<AzureResourceDefinitionV1> Resources { get; set; }
    }
}