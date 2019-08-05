namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class CosmosDbMetricDefinition : MetricDefinition
    {
        public string DbName { get; set; }

        public override ResourceType ResourceType { get; } = this.ResourceType.CosmosDb;
    }
}
