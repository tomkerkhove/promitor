namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a Mongo cluster.
    /// </summary>
    public class MongoClusterResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The mongo cluster name.
        /// </summary>
        public string ClusterName { get; set; }
    }
}