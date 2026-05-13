namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Private Link service.
    /// </summary>
    public class PrivateLinkServiceResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Private Link service to get metrics for.
        /// </summary>
        public string PrivateLinkServiceName { get; set; }
    }
}
