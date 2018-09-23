using Promitor.Scraper.Host.Configuration.Model;
using GuardNet;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Host.Configuration.Serialization
{
    internal class AzureMetadataDeserializer : Deserializer<AzureMetadata>
    {
        internal override AzureMetadata Deserialize(YamlMappingNode node)
        {
            Guard.NotNull(node, nameof(node));

            var tenantId = node.Children[new YamlScalarNode("tenantId")];
            var subscriptionId = node.Children[new YamlScalarNode("subscriptionId")];
            var resourceGroupName = node.Children[new YamlScalarNode("resourceGroupName")];

            var azureMetadata = new AzureMetadata
            {
                TenantId = tenantId?.ToString(),
                SubscriptionId = subscriptionId?.ToString(),
                ResourceGroupName = resourceGroupName?.ToString()
            };

            return azureMetadata;
        }
    }
}