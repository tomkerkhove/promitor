using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model
{
    /// <summary>
    /// Contains the definition for a prometheus metric, along with the resources
    /// that should be scraped to supply the metric.
    /// </summary>
    public class MetricDefinitionV2
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
        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Contains the configuration used when querying Azure metrics.
        /// </summary>
        public AzureMetricConfigurationV2 AzureMetricConfiguration { get; set; } = new AzureMetricConfigurationV2();

        /// <summary>
        /// Allows a custom scraping schedule to be specified for the metric.
        /// </summary>
        public ScrapingV2 Scraping { get; set; } = new ScrapingV2();

        /// <summary>
        /// The resources to be scraped.
        /// </summary>
        public List<AzureResourceDefinitionV2> Resources { get; set; } = new List<AzureResourceDefinitionV2>();
    }
}