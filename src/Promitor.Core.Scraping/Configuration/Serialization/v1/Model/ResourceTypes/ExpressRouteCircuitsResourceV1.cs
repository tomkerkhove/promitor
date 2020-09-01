namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Express Route circuit.
    /// </summary>
    public class ExpressRouteCircuitsResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the express route circuits to get metrics for.
        /// </summary>
        public string ExpressRouteCircuitsName { get; set; }
    }
}
