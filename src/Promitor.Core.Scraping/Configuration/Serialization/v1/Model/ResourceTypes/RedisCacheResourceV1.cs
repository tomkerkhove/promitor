namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    public class RedisCacheResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the redis cache.
        /// </summary>
        public string CacheName { get; set; }
    }
}
