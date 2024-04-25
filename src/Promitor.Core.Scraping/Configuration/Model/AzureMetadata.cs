using Promitor.Core.Serialization.Enum;

namespace Promitor.Core.Scraping.Configuration.Model
{
    public class AzureMetadata
    {
        public string ResourceGroupName { get; set; }
        public string SubscriptionId { get; set; }
        public string TenantId { get; set; }
        public AzureCloud Cloud { get; set; }
    }
}