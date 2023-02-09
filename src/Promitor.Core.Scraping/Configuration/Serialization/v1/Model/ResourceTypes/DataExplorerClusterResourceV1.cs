namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a Azure Data Explorer cluster.
    /// </summary>
    public class DataExplorerClusterResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Data Explorer Cluster to get metrics for.
        /// </summary>
        public string ClusterName { get; set; }
    }
}
