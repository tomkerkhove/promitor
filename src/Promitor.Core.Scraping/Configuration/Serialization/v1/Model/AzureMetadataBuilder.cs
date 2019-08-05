using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class AzureMetadataBuilder
    {
        public string ResourceGroupName { get; set; }
        public string SubscriptionId { get; set; }
        public string TenantId { get; set; }

        public AzureMetadata Build()
        {
            return new AzureMetadata
            {
                ResourceGroupName = ResourceGroupName,
                SubscriptionId = SubscriptionId,
                TenantId = TenantId
            };
        }
    }
}