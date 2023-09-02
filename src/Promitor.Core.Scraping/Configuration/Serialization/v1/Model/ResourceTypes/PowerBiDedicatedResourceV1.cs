namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape PowerBI Dedicated
    /// </summary>
    public class PowerBiDedicatedResourceV1 : AzureResourceDefinitionV1
    {
    	/// <summary>
        /// The name of the PowerBI Dedicated Capacity to get metrics for.
        /// </summary>
        public string CapacityName { get; set; }
    }
}
