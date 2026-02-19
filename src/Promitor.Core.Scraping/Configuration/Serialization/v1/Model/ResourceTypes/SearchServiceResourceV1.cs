namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure AI Search service.
    /// </summary>
    public class SearchServiceResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure AI Search service to get metrics for.
        /// </summary>
        public string SearchServiceName { get; set; }
    }
}
