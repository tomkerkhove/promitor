namespace Promitor.Core.Contracts.ResourceTypes
{
    public class ServiceBusNamespaceResourceDefinition : AzureResourceDefinition
    {
        public ServiceBusNamespaceResourceDefinition(string subscriptionId, string resourceGroupName, string @namespace, string queueName, string topicName)
            : base(ResourceType.ServiceBusNamespace, subscriptionId, resourceGroupName, @namespace, $"{@namespace}-{queueName}-{topicName}")
        {
            Namespace = @namespace;
            QueueName = queueName;
            TopicName = topicName;
        }

        public string Namespace { get; }
        public string QueueName { get; }
        public string TopicName { get; }
    }
}