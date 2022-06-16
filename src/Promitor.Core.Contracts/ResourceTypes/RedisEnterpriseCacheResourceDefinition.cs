namespace Promitor.Core.Contracts.ResourceTypes
{
    public class RedisEnterpriseCacheResourceDefinition : AzureResourceDefinition
    {
        public RedisEnterpriseCacheResourceDefinition(string subscriptionId, string resourceGroupName, string cacheName)
            : base(ResourceType.RedisEnterpriseCache, subscriptionId, resourceGroupName, cacheName)
        {
            CacheName = cacheName;
        }

        public string CacheName { get; }
    }
}
