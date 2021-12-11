namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Load Balancer.
    /// </summary>
    public class LoadBalancerResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Load Balancer to get metrics for.
        /// </summary>
        public string LoadBalancerName { get; set; }
    }
}
