using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    internal class AzureMetadataDeserializer : Deserializer<AzureMetadataBuilder>
    {
        internal AzureMetadataDeserializer(ILogger logger) : base(logger)
        {
        }

        internal override AzureMetadataBuilder Deserialize(YamlMappingNode node)
        {
            Guard.NotNull(node, nameof(node));

            var tenantId = node.Children[new YamlScalarNode("tenantId")];
            var subscriptionId = node.Children[new YamlScalarNode("subscriptionId")];
            var resourceGroupName = node.Children[new YamlScalarNode("resourceGroupName")];

            var azureMetadata = new AzureMetadataBuilder
            {
                TenantId = tenantId?.ToString(),
                SubscriptionId = subscriptionId?.ToString(),
                ResourceGroupName = resourceGroupName?.ToString()
            };

            return azureMetadata;
        }
    }
}