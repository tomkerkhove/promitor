using System;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureMetadataDeserializer : Deserializer<AzureMetadataV1>
    {
        private const string CloudTag = "cloud";
        private const string TenantIdTag = "tenantId";
        private const string SubscriptionIdTag = "subscriptionId";
        private const string ResourceGroupNameTag = "resourceGroupName";

        public AzureMetadataDeserializer(ILogger<AzureMetadataDeserializer> logger) : base(logger)
        {
        }

        public override AzureMetadataV1 Deserialize(YamlMappingNode node)
        {
            var metadata = new AzureMetadataV1();

            var azureCloud = node.GetEnum<AzureCloudsV1>(CloudTag);
            var cloud = DetermineAzureCloud(azureCloud);

            metadata.TenantId = node.GetString(TenantIdTag);
            metadata.SubscriptionId = node.GetString(SubscriptionIdTag);
            metadata.ResourceGroupName = node.GetString(ResourceGroupNameTag);
            metadata.Cloud = cloud;

            return metadata;
        }

        private AzureEnvironment DetermineAzureCloud(AzureCloudsV1? azureCloud)
        {
            if (azureCloud == null)
            {
                return AzureEnvironment.AzureGlobalCloud;
            }

            switch (azureCloud)
            {
                case AzureCloudsV1.Global:
                    return AzureEnvironment.AzureGlobalCloud;
                case AzureCloudsV1.China:
                    return AzureEnvironment.AzureChinaCloud;
                case AzureCloudsV1.Germany:
                    return AzureEnvironment.AzureGermanCloud;
                case AzureCloudsV1.UsGov:
                    return AzureEnvironment.AzureUSGovernment;
                default:
                    throw new ArgumentOutOfRangeException(nameof(azureCloud), azureCloud, $"{azureCloud} is not supported yet");
            }
        }
    }
}
