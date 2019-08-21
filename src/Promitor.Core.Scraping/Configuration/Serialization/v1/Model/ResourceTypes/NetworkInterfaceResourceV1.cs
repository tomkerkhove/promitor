namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    public class NetworkInterfaceResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the network interface to scrape.
        /// </summary>
        public string NetworkInterfaceName { get; set; }
    }
}
