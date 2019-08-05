using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using YamlDotNet.Serialization;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics
{
    public abstract class MetricDefinitionBuilder
    {
        /// <summary>
        ///     Configuration about the Azure Monitor metric to scrape
        /// </summary>
        [YamlMember(Alias = "azureMetricConfiguration")]
        public AzureMetricConfigurationBuilder AzureMetricConfigurationBuilder { get; set; } = new AzureMetricConfigurationBuilder();

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
        [YamlMember(Alias = "scraping")]
        public ScrapingBuilder ScrapingBuilder { get; set; } = new ScrapingBuilder();

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
