namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure CDN.
    /// </summary>
    public class CdnResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure CDN instance.
        /// </summary>
        public string CdnName { get; set; }
    }
}
