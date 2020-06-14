﻿using System.Collections.Generic;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureMetadataDeserializer : Deserializer<AzureMetadataV1>
    {
        public AzureMetadataDeserializer(ILogger<AzureMetadataDeserializer> logger) : base(logger)
        {
            Map(metadata => metadata.TenantId)
                .IsRequired();
            Map(metadata => metadata.SubscriptionId)
                .IsRequired();
            Map(metadata => metadata.ResourceGroupName)
                .IsRequired();
            Map(metadata => metadata.Cloud)
                .WithDefault(AzureEnvironment.AzureGlobalCloud)
                .MapUsing(DetermineAzureCloud);
        }

        private object DetermineAzureCloud(string rawAzureCloud, KeyValuePair<YamlNode, YamlNode> nodePair, IErrorReporter errorReporter)
        {
            if (System.Enum.TryParse<AzureCloudsV1>(rawAzureCloud, out var azureCloud))
            {
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
                }
            }
            else
            {
                errorReporter.ReportError(nodePair.Value, $"'{rawAzureCloud}' is not a valid value for 'cloud'.");
            }

            return null;
        }
    }
}
