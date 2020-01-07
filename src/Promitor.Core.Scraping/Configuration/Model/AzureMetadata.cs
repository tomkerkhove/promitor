using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace Promitor.Core.Scraping.Configuration.Model
{
    public class AzureMetadata
    {
        public string ResourceGroupName { get; set; }
        public string SubscriptionId { get; set; }
        public string TenantId { get; set; }
        public AzureEnvironment Cloud { get; set; }
    }
}