namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a express route circuits.
    /// </summary>
    public class ExpressRouteCircuitsResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the express route circuits to get metrics for.
        /// </summary>
        public string ExpressRouteCircuitsName { get; set; }
    }
}
