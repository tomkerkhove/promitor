namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Traffic Manager Profile.
    /// </summary>
    public class TrafficManagerResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Traffic Manager Profile to get metrics for.
        /// </summary>
        public string Name { get; set; }
    }
}
