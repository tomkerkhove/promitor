using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model
{
    public class AzureMetadataV2
    {
        public string TenantId { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
    }
}
