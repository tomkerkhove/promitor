namespace Promitor.Core.Contracts.ResourceTypes
{
    public class EventHubResourceDefinition : AzureResourceDefinition
    {
        public EventHubResourceDefinition(string subscriptionId, string resourceGroupName, string eventHubNamespace, string topicName)
            : base(ResourceType.EventHubs, subscriptionId, resourceGroupName, eventHubNamespace, $"{eventHubNamespace}-{topicName}")
        {
            Namespace = eventHubNamespace;
            TopicName = topicName;
        }

        public string Namespace { get; }
        public string TopicName { get; }
    }
}