﻿namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class RedisCacheResourceDefinition : AzureResourceDefinition
    {
        public RedisCacheResourceDefinition(string subscriptionId, string resourceGroupName, string cacheName)
            : base(ResourceType.RedisCache, subscriptionId, resourceGroupName)
        {
            CacheName = cacheName;
        }

        public string CacheName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => CacheName;
    }
}
