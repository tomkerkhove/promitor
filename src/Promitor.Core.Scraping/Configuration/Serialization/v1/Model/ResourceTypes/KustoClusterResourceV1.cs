namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a kusto cluster.
    /// </summary>
    public class KustoClusterResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Kusto Cluster to get metrics for.
        /// </summary>
        public string KustoClusterName { get; set; }
    }
}
