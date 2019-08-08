using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Core
{
    public class AzureMetadataDeserializer : Deserializer<AzureMetadataV2>
    {
        private const string TenantIdTag = "tenantId";
        private const string SubscriptionIdTag = "subscriptionId";
        private const string ResourceGroupNameTag = "resourceGroupName";

        public AzureMetadataDeserializer(ILogger logger) : base(logger)
        {
        }

        public override AzureMetadataV2 Deserialize(YamlMappingNode node)
        {
            var metadata = new AzureMetadataV2();

            metadata.TenantId = GetString(node, TenantIdTag);
            metadata.SubscriptionId = GetString(node, SubscriptionIdTag);
            metadata.ResourceGroupName = GetString(node, ResourceGroupNameTag);
            
            return metadata;
        }
    }
}
