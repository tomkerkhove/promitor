namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class StorageQueueMetricDefinition : MetricDefinition
    {
        public string AccountName { get; set; }
        public string QueueName { get; set; }
        public Secret SasToken { get; set; }
        public override ResourceType ResourceType { get; } = this.ResourceType.StorageQueue;
    }
}