using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class RedisCacheMetricDefinition : MetricDefinition
    {
        public RedisCacheMetricDefinition()
        {
        }

        public RedisCacheMetricDefinition(AzureMetricConfiguration azureMetricConfiguration, string description, string name, string resourceGroupName, string cacheName, Dictionary<string, string> labels, Scraping scraping)
            : base(azureMetricConfiguration, description, name, resourceGroupName, labels, scraping)
        {
            CacheName = cacheName;
        }

        public string CacheName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.RedisCache;
    }
}
