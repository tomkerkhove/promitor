namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class RedisCacheMetricDefinition : AzureResourceDefinition
    {
        public RedisCacheMetricDefinition() : base(ResourceType.RedisCache)
        {
        }

        public RedisCacheMetricDefinition(string resourceGroupName, string cacheName)
            : base(ResourceType.RedisCache, resourceGroupName)
        {
            CacheName = cacheName;
        }

        public string CacheName { get; set; }
    }
}
