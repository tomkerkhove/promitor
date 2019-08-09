using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics
{
    public abstract class MetricDefinitionV1
    {
        /// <summary>
        ///     Configuration about the Azure Monitor metric to scrape
        /// </summary>
        public AzureMetricConfigurationV1 AzureMetricConfiguration { get; set; } = new AzureMetricConfigurationV1();

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
        public ScrapingV1 Scraping { get; set; } = new ScrapingV1();

        public abstract MetricDefinition Build();
        
        /// <summary>
        /// Provided as a convenience to cast the result of <see cref="Build"/>
        /// to the correct type.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="MetricDefinition"/> to be returned.</typeparam>
        /// <returns>The definition.</returns>
        public T Build<T>() where T: MetricDefinition
        {
            return Build() as T;
        }
    }
}
