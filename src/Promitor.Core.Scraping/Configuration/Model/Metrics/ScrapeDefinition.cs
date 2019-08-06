namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    /// <summary>
    /// Defines an individual Azure resource to be scraped.
    /// </summary>
    public class ScrapeDefinition<TResourceDefinition> where TResourceDefinition: AzureResourceDefinition
    {
        public ScrapeDefinition(
            AzureMetricConfiguration azureMetricConfiguration,
            PrometheusMetricDefinition prometheusMetricDefinition,
            Scraping scraping,
            TResourceDefinition resource)
        {
            AzureMetricConfiguration = azureMetricConfiguration;
            PrometheusMetricDefinition = prometheusMetricDefinition;
            Scraping = scraping;
            Resource = resource;
        }

        /// <summary>
        ///     Configuration about the Azure Monitor metric to scrape
        /// </summary>
        public AzureMetricConfiguration AzureMetricConfiguration { get; }

        public PrometheusMetricDefinition PrometheusMetricDefinition { get; }

        /// <summary>
        /// Gets or sets the scraping model.
        /// </summary>
        public Scraping Scraping { get; }

        /// <summary>
        /// Gets or sets the resource to scrape.
        /// </summary>
        public TResourceDefinition Resource { get; }
    }
}
