namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape PowerBi Embedded
    /// </summary>
    public class PowerBiEmbeddedResourceV1 : AzureResourceDefinitionV1
	{
        /// <summary>
        /// The name of the PowerBi Embedded Capacity to get metrics for.
        /// </summary>
        public string CapacityName { get; set; }
	}
}