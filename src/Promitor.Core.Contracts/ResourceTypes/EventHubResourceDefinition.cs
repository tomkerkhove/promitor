namespace Promitor.Core.Contracts.ResourceTypes
{
    public class EventHubResourceDefinition : AzureResourceDefinition
    {
        public EventHubResourceDefinition(string subscriptionId, string resourceGroupName, string @namespace, string topicName)
            : base(ResourceType.EventHubs, subscriptionId, resourceGroupName, @namespace, $"{@namespace}-{topicName}")
        {
            Namespace = @namespace;
            TopicName = topicName;
        }

        public string Namespace { get; }
        public string TopicName { get; }
    }
}