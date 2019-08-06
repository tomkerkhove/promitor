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
            List<AzureResourceDefinition> resources)
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
        public List<AzureResourceDefinition> Resources { get; set; }

        /// <summary>
        /// Creates a <see cref="ScrapeDefinition{TResourceDefinition}"/> object for the specified resource.
        /// </summary>
        /// <param name="resource">The resource to scrape.</param>
        /// <returns>The scrape definition.</returns>
        public ScrapeDefinition<AzureResourceDefinition> CreateScrapeDefinition(AzureResourceDefinition resource)
        {
            return new ScrapeDefinition<AzureResourceDefinition>(
                AzureMetricConfiguration,
                PrometheusMetricDefinition,
                Scraping,
                resource);
        }
    }
}
