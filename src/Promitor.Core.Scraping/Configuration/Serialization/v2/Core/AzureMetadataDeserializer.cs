using System;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Core
{
    public class AzureMetadataDeserializer : IDeserializer<AzureMetadataV2>
    {
        private const string TenantIdTag = "tenantId";
        private const string SubscriptionIdTag = "subscriptionId";
        private const string ResourceGroupNameTag = "resourceGroupName";

        public AzureMetadataV2 Deserialize(YamlNode node)
        {
            Guard.NotNull(node, nameof(node));

            var mappingNode = node as YamlMappingNode;
            if (mappingNode == null)
            {
                throw new ArgumentException(
                    $"Expected a YamlMappingNode but received '{node.GetType()}'", nameof(node));
            }

            var metadata = new AzureMetadataV2();

            if (mappingNode.Children.TryGetValue(TenantIdTag, out var tenantIdNode))
            {
                metadata.TenantId = tenantIdNode.ToString();
            }

            if (mappingNode.Children.TryGetValue(SubscriptionIdTag, out var subscriptionIdNode))
            {
                metadata.SubscriptionId = subscriptionIdNode.ToString();
            }

            if (mappingNode.Children.TryGetValue(ResourceGroupNameTag, out var resourceGroupNameNode))
            {
                metadata.ResourceGroupName = resourceGroupNameNode.ToString();
            }
            
            return metadata;
        }
    }
}
