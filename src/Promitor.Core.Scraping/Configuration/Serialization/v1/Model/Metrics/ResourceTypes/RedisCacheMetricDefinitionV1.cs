using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class RedisCacheMetricDefinitionV1 : MetricDefinitionV1
    {
        public string CacheName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.RedisCache;

        public override MetricDefinition Build()
        {
            return new RedisCacheMetricDefinition(
                AzureMetricConfiguration.Build(),
                Description,
                Name,
                ResourceGroupName,
                CacheName,
                Labels,
                Scraping.Build());
        }
    }
}
