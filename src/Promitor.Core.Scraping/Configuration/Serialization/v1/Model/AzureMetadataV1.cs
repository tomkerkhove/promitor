using Promitor.Agents.Core.Configuration;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Serialization.Enum;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class AzureMetadataV1
    {
        public string TenantId { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public AzureCloud Cloud { get; set; }
        public AzureEndpointsV1 Endpoints { get; set; }
    }
}
