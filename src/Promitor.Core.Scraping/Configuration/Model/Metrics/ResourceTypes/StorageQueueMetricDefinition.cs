namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class StorageQueueMetricDefinition: MetricDefinition
    {
        public string AccountName { get; set; }
        public string QueueName { get; set; }
        public string SasToken { get; set; }
        public override ResourceType ResourceType { get; set; } = ResourceType.StorageQueue;
    }
}