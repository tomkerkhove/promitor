using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class AzureMetadataV1
    {
        public string TenantId { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public AzureEnvironment Cloud { get; set; }
    }
}
