using System.Collections.Generic;

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
            ResourceType resourceType,
            List<IAzureResourceDefinition> resources)
        {
            AzureMetricConfiguration = azureMetricConfiguration;
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
        /// Gets or sets the list of resources to scrape.
        /// </summary>
        public List<IAzureResourceDefinition> Resources { get; set; }

        /// <summary>
        /// Creates a <see cref="ScrapeDefinition{TResourceDefinition}"/> object for the specified resource.
        /// </summary>
        /// <param name="resource">The resource to scrape.</param>
        /// <param name="azureMetadata">The Azure global metadata.</param>
        /// <returns>The scrape definition.</returns>
        public ScrapeDefinition<IAzureResourceDefinition> CreateScrapeDefinition(IAzureResourceDefinition resource, AzureMetadata azureMetadata)
        {
            return new ScrapeDefinition<IAzureResourceDefinition>(
                AzureMetricConfiguration,
                PrometheusMetricDefinition,
                Scraping,
                resource,
                string.IsNullOrEmpty(resource.ResourceGroupName) ? azureMetadata.ResourceGroupName : resource.ResourceGroupName);
        }
    }
}
