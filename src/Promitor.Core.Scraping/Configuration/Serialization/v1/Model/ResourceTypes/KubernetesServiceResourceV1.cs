namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Kubernetes Service.
    /// </summary>
    public class KubernetesServiceResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Kubernetes Service to get metrics for.
        /// </summary>
        public string ClusterName { get; set; }
    }
}
