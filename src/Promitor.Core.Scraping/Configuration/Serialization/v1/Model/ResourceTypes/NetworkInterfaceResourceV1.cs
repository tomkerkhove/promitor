namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a Network Interface.
    /// </summary>
    public class NetworkInterfaceResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the network interface to scrape.
        /// </summary>
        public string NetworkInterfaceName { get; set; }
    }
}
