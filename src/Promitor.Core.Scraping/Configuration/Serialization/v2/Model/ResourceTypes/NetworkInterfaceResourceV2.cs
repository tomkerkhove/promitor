namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes
{
    public class NetworkInterfaceResourceV2 : AzureResourceDefinitionV2
    {
        /// <summary>
        /// The name of the network interface to scrape.
        /// </summary>
        public string NetworkInterfaceName { get; set; }
    }
}
