using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

// ReSharper disable All

namespace Promitor.Core.Scraping
{
    /// <summary>
    /// Scrapes an Azure IoT Hub
    /// </summary>
    public class IoTHubScraper : AzureMonitorScraper<IoTHubResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Devices/IotHubs/{2}";

        public IoTHubScraper(ScraperConfiguration scraperConfiguration) : 
            base(scraperConfiguration)
        {
        }

        /// <inheritdoc />
        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, IoTHubResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.IoTHubName);
        }
    }
}