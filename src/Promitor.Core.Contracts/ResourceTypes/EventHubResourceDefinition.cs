namespace Promitor.Core.Contracts.ResourceTypes
{
    public class EventHubResourceDefinition : AzureResourceDefinition
    {
        public EventHubResourceDefinition(string subscriptionId, string resourceGroupName, string eventHubsNamespace, string topicName)
            : base(ResourceType.EventHubs, subscriptionId, resourceGroupName, eventHubsNamespace, $"{eventHubsNamespace}-{topicName}")
        {
            Namespace = eventHubsNamespace;
            TopicName = topicName;
        }

        public string Namespace { get; }
        public string TopicName { get; }
    }
}