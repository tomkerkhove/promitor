namespace Promitor.Scraper.Model.Configuration.Metrics.ResouceTypes
{
    public class ServiceBusQueueMetricDefinition : MetricDefinition
    {
        public string Namespace { get; set; }
        public string QueueName { get; set; }
        public override ResourceType ResourceType { get; set; } = ResourceType.ServiceBusQueue;
    }
}