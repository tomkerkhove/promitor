namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a Public IP Address.
    /// </summary>
    public class PublicIpAddressResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Public IP Address to get metrics for.
        /// </summary>
        public string PublicIpAddressName { get; set; }
    }
}
