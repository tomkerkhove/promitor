namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Cache for Redis (Enterprise).
    /// </summary>
    public class RedisEnterpriseCacheResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Cache for Redis (Enterprise) instance.
        /// </summary>
        public string CacheName { get; set; }
    }
}
