namespace Promitor.Core.Contracts.ResourceTypes
{
    public class ServiceBusQueueResourceDefinition : AzureResourceDefinition
    {
        public ServiceBusQueueResourceDefinition(string subscriptionId, string resourceGroupName, string @namespace, string queueName)
            : base(ResourceType.ServiceBusQueue, subscriptionId, resourceGroupName, @namespace, $"{@namespace}-{queueName}")
        {
            Namespace = @namespace;
            QueueName = queueName;
        }

        public string Namespace { get; }
        public string QueueName { get; }
    }
}