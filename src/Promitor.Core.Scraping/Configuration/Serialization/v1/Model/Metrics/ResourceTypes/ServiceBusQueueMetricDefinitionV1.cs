using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class ServiceBusQueueMetricDefinitionV1 : MetricDefinitionV1
    {
        public string Namespace { get; set; }
        public string QueueName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.ServiceBusQueue;
    }
}