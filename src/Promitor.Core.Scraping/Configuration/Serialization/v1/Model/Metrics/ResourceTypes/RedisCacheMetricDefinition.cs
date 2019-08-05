namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class RedisCacheMetricDefinition : MetricDefinition
    {
        public string CacheName { get; set; }
        public override ResourceType ResourceType { get; } = this.ResourceType.RedisCache;
    }
}
