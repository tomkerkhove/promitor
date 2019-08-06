namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ServiceBusQueueMetricDefinition : AzureResourceDefinition
    {
        public ServiceBusQueueMetricDefinition() : base(ResourceType.ServiceBusQueue)
        {
        }

        public ServiceBusQueueMetricDefinition(string resourceGroupName, string ns, string queueName)
            : base(ResourceType.ServiceBusQueue, resourceGroupName)
        {
            Namespace = ns;
            QueueName = queueName;
        }

        public string Namespace { get; set; }
        public string QueueName { get; set; }
    }
}