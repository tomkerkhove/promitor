using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureMetadataDeserializer : Deserializer<AzureMetadataV1>
    {
        private const string TenantIdTag = "tenantId";
        private const string SubscriptionIdTag = "subscriptionId";
        private const string ResourceGroupNameTag = "resourceGroupName";

        public AzureMetadataDeserializer(ILogger<AzureMetadataDeserializer> logger) : base(logger)
        {
        }

        public override AzureMetadataV1 Deserialize(YamlMappingNode node)
        {
            var metadata = new AzureMetadataV1();

            metadata.TenantId = node.GetString(TenantIdTag);
            metadata.SubscriptionId = node.GetString(SubscriptionIdTag);
            metadata.ResourceGroupName = node.GetString(ResourceGroupNameTag);
            
            return metadata;
        }
    }
}
