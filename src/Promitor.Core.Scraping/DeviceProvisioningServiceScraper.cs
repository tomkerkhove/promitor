using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

// ReSharper disable All

namespace Promitor.Core.Scraping
{
    /// <summary>
    /// Scrapes an Azure IoT Hub Device Provisioning Service (DPS)
    /// </summary>
    public class DeviceProvisioningServiceScraper : AzureMonitorScraper<DeviceProvisioningServiceResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Devices/provisioningServices/{2}";

        public DeviceProvisioningServiceScraper(ScraperConfiguration scraperConfiguration) :
            base(scraperConfiguration)
        {
        }

        /// <inheritdoc />
        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, DeviceProvisioningServiceResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.DeviceProvisioningServiceName);
        }
    }
}