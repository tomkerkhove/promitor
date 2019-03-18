namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ServiceBusQueueMetricDefinition : MetricDefinition
    {
        public string Namespace { get; set; }
        public string QueueName { get; set; }
        public override ResourceType ResourceType { get; set; } = ResourceType.ServiceBusQueue;
    }
}