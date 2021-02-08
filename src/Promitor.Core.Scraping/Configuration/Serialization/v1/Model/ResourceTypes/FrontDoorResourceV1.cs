namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Front Door resource.
    /// </summary>
    public class FrontDoorResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Front Door resource to get metrics for.
        /// </summary>
        public string Name { get; set; }
    }
}