using GuardNet;

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
        /// <param name="prometheusMetricDefinition">The details of the prometheus metric that will be created.</param>
        /// <param name="scraping">The scraping model.</param>
        /// <param name="resource">The resource to scrape.</param>
        /// <param name="resourceGroupName">
        /// The name of the resource group containing the resource to scrape. This should contain the global
        /// resource group name if none is overridden at the resource level.
        /// </param>
        public ScrapeDefinition(
            AzureMetricConfiguration azureMetricConfiguration,
            PrometheusMetricDefinition prometheusMetricDefinition,
            Scraping scraping,
            TResourceDefinition resource,
            string resourceGroupName)
        {
            Guard.NotNull(azureMetricConfiguration, nameof(azureMetricConfiguration));
            Guard.NotNull(prometheusMetricDefinition, nameof(prometheusMetricDefinition));
            Guard.NotNull(scraping, nameof(scraping));
            Guard.NotNull(resource, nameof(resource));
            Guard.NotNull(resourceGroupName, nameof(resourceGroupName));

            AzureMetricConfiguration = azureMetricConfiguration;
            PrometheusMetricDefinition = prometheusMetricDefinition;
            Scraping = scraping;
            Resource = resource;
            ResourceGroupName = resourceGroupName;
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
        /// The resource to scrape.
        /// </summary>
        public TResourceDefinition Resource { get; }

        /// <summary>
        /// The Azure resource group to get the metric from. This should be used instead of using
        /// the ResourceGroupName from <see cref="Resource"/> because this property will contain
        /// the global resource group name if none is overridden at the resource level.
        /// </summary>
        public string ResourceGroupName { get; }
    }
}
