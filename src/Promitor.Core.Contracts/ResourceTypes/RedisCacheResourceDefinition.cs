namespace Promitor.Core.Contracts.ResourceTypes
{
    public class RedisCacheResourceDefinition : AzureResourceDefinition
    {
        public RedisCacheResourceDefinition(string subscriptionId, string resourceGroupName, string cacheName)
            : base(ResourceType.RedisCache, subscriptionId, resourceGroupName, cacheName)
        {
            CacheName = cacheName;
        }

        public string CacheName { get; }
    }
}
