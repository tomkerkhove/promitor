namespace Promitor.Core.Contracts.ResourceTypes
{
    public class PrivateLinkServiceResourceDefinition : AzureResourceDefinition
    {
        public PrivateLinkServiceResourceDefinition(string subscriptionId, string resourceGroupName, string privateLinkServiceName)
            : base(ResourceType.PrivateLinkService, subscriptionId, resourceGroupName, privateLinkServiceName)
        {
            PrivateLinkServiceName = privateLinkServiceName;
        }

        public string PrivateLinkServiceName { get; }
    }
}
