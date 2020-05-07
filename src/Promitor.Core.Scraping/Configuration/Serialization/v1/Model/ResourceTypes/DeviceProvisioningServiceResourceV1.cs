namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure IoT Hub Device Provisioning Service (DPS).
    /// </summary>
    public class DeviceProvisioningServiceResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure IoT Hub Device Provisioning Service (DPS) to get metrics for.
        /// </summary>
        public string DeviceProvisioningServiceName { get; set; }
    }
}
