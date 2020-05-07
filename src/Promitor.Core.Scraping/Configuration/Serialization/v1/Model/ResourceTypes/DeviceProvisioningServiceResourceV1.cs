namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Device Provisioning Service.
    /// </summary>
    public class DeviceProvisioningServiceResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Device Provisioning Service to get metrics for.
        /// </summary>
        public string DeviceProvisioningServiceName { get; set; }
    }
}
