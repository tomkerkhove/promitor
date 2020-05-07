namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure IoT Hub.
    /// </summary>
    public class IoTHubResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure IoT Hub to get metrics for.
        /// </summary>
        public string IoTHubName { get; set; }
    }
}
