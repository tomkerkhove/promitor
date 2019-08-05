using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    public abstract class MetricDefinition
    {
        protected MetricDefinition()
        {
        }

        protected MetricDefinition(
            AzureMetricConfiguration azureMetricConfiguration,
            string description,
            string name,
            string resourceGroupName,
            Dictionary<string, string> labels,
            Scraping scraping)
        {
            AzureMetricConfiguration = azureMetricConfiguration;
            Description = description;
            Name = name;
            ResourceGroupName = resourceGroupName;
            Labels = labels;
            Scraping = scraping;
        }

        /// <summary>
        ///     Configuration about the Azure Monitor metric to scrape
        /// </summary>
        public AzureMetricConfiguration AzureMetricConfiguration { get; set; }

        /// <summary>
        ///     Description concerning metric that will be made available in the scraping endpoint
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Name of the metric to use when exposing in the scraping endpoint
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Specify a resource group to scrape that defers from the default resource group.
        ///     This enables you to do multi-resource group scraping with one configuration file.
        /// </summary>
        public string ResourceGroupName { get; set; }

        /// <summary>
        ///     Type of resource that is configured
        /// </summary>
        public abstract ResourceType ResourceType { get; }

        /// <summary>
        ///     Collection of custom labels to add to every metric
        /// </summary>
        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the scraping model.
        /// </summary>
        public Scraping Scraping { get; set; } = new Scraping();
    }
}
