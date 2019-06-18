namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class RedisCacheMetricDefinition : MetricDefinition
    {
        public string CacheName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.RedisCache;
    }
}
