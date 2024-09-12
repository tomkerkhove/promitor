namespace Promitor.Core.Contracts.ResourceTypes
{
    public class AzureFirewallDefinition : AzureResourceDefinition
    {
        public AzureFirewallDefinition(string subscriptionId, string resourceGroupName, string azureFirewallName)
            : base(ResourceType.AzureFirewall, subscriptionId, resourceGroupName, azureFirewallName)
        {
            AzureFirewallName = azureFirewallName;
        }

        public string AzureFirewallName { get; }
    }
}
