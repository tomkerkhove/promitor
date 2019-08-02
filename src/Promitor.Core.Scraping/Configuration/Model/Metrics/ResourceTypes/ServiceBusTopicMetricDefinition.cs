namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ServiceBusTopicMetricDefinition : MetricDefinition
    {
        public string Namespace { get; set; }
        public string TopicName { get; set; }
        public string SubscriptionName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.ServiceBusTopic;
    }
}