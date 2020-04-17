namespace Promitor.Agents.ResourceDiscovery.Graph.Model
{
    public class Resource
    {
        public Resource(string subscriptionId, string resourceGroup, string type, string name)
        {
            SubscriptionId = subscriptionId;
            ResourceGroup = resourceGroup;
            Name = name;
            Type = type;
        }

        public string SubscriptionId { get; }
        public string ResourceGroup { get; }
        public string Name { get; }
        public string Type { get; }
    }
}
