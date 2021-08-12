namespace Promitor.Agents.ResourceDiscovery.Graph.Model
{
    public class AzureResourceGroupInformation
    {
        public string TenantId { get; set; }
        public string SubscriptionId { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string ManagedBy { get; set; }
        public string ProvisioningState { get; set; }
    }
}
