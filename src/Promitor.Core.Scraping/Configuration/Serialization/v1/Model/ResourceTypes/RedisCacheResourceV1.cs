namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a redis cache.
    /// </summary>
    public class RedisCacheResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the redis cache.
        /// </summary>
        public string CacheName { get; set; }
    }
}
