namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Virtual Network.
    /// </summary>
    public class VirtualNetworkResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Virtual Network to get metrics for.
        /// </summary>
        public string VirtualNetworkName { get; set; }
    }
}
