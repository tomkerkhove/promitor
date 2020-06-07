namespace Promitor.Core.Contracts.ResourceTypes
{
    public class ServiceBusQueueResourceDefinition : AzureResourceDefinition
    {
        public ServiceBusQueueResourceDefinition(string subscriptionId, string resourceGroupName, string ns, string queueName)
            : base(ResourceType.ServiceBusQueue, subscriptionId, resourceGroupName, ns, $"{ns}-{queueName}")
        {
            Namespace = ns;
            QueueName = queueName;
        }

        public string Namespace { get; }
        public string QueueName { get; }
    }
}