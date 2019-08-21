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

        public AzureMetadataDeserializer(ILogger logger) : base(logger)
        {
        }

        public override AzureMetadataV1 Deserialize(YamlMappingNode node)
        {
            var metadata = new AzureMetadataV1();

            metadata.TenantId = GetString(node, TenantIdTag);
            metadata.SubscriptionId = GetString(node, SubscriptionIdTag);
            metadata.ResourceGroupName = GetString(node, ResourceGroupNameTag);
            
            return metadata;
        }
    }
}
