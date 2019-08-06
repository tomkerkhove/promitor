namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ServiceBusQueueResourceDefinition : AzureResourceDefinition
    {
        public ServiceBusQueueResourceDefinition() : base(ResourceType.ServiceBusQueue)
        {
        }

        public ServiceBusQueueResourceDefinition(string resourceGroupName, string ns, string queueName)
            : base(ResourceType.ServiceBusQueue, resourceGroupName)
        {
            Namespace = ns;
            QueueName = queueName;
        }

        public string Namespace { get; set; }
        public string QueueName { get; set; }
    }
}