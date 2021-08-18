namespace Promitor.Agents.ResourceDiscovery.Graph.Model
{
    public class AzureSubscriptionInformation
    {
        public string TenantId { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string State { get; set; }
        public string SpendingLimit { get; set; }
        public string QuotaId { get; set; }
        public string AuthorizationSource { get; set; }
    }
}
