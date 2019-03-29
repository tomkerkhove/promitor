namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class CosmosDbMetricDefinition : MetricDefinition
    {
        public string DbName { get; set; }

        public override ResourceType ResourceType { get; } = ResourceType.CosmosDb;
    }
}
