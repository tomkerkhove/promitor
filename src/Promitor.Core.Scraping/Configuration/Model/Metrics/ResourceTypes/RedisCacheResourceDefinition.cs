namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class RedisCacheResourceDefinition : AzureResourceDefinition
    {
        public RedisCacheResourceDefinition() : base(ResourceType.RedisCache)
        {
        }

        public RedisCacheResourceDefinition(string resourceGroupName, string cacheName)
            : base(ResourceType.RedisCache, resourceGroupName)
        {
            CacheName = cacheName;
        }

        public string CacheName { get; set; }
    }
}
