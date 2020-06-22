namespace Promitor.Core.Contracts.ResourceTypes
{
    public class ServiceBusQueueResourceDefinition : AzureResourceDefinition
    {
        public ServiceBusQueueResourceDefinition(string subscriptionId, string resourceGroupName, string serviceBusNamespace, string queueName)
            : base(ResourceType.ServiceBusQueue, subscriptionId, resourceGroupName, serviceBusNamespace, $"{serviceBusNamespace}-{queueName}")
        {
            Namespace = serviceBusNamespace;
            QueueName = queueName;
        }

        public string Namespace { get; }
        public string QueueName { get; }
    }
}