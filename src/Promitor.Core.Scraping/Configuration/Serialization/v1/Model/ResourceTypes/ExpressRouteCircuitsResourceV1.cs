namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Express Route circuit.
    /// </summary>
    public class ExpressRouteCircuitResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Express Route circuit to get metrics for.
        /// </summary>
        public string ExpressRouteCircuitName { get; set; }
    }
}
