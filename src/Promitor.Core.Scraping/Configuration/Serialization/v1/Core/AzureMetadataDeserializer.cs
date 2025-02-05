using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration;
using Promitor.Core.Extensions;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Serialization.Enum;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureMetadataDeserializer : Deserializer<AzureMetadataV1>
    {
        private readonly IDeserializer<AzureEndpointsV1> _azureEndpointsDeserializer;
        private AzureCloud _azureCloud;

        public AzureMetadataDeserializer(
            IDeserializer<AzureEndpointsV1> azureEndpointsDeserializer,
            ILogger<AzureMetadataDeserializer> logger) : base(logger)
        {
            _azureEndpointsDeserializer = azureEndpointsDeserializer;

            Map(metadata => metadata.TenantId)
                .IsRequired();
            Map(metadata => metadata.SubscriptionId)
                .IsRequired();
            Map(metadata => metadata.ResourceGroupName)
                .IsRequired();
            Map(metadata => metadata.Cloud)
                .WithDefault(AzureCloud.Global)
                .MapUsing(DetermineAzureCloud);
            Map(metadata => metadata.Endpoints)
                .MapUsing(DeserializeEndpoints);
        }

        private object DetermineAzureCloud(string rawAzureCloud, KeyValuePair<YamlNode, YamlNode> nodePair, IErrorReporter errorReporter)
        {
            if (Enum.TryParse<AzureCloud>(rawAzureCloud, out var azureCloud))
            {
                _azureCloud = azureCloud;
                try
                {
                    azureCloud.ValidateMetricsClientAudience();
                    return azureCloud;
                }
                catch (ArgumentOutOfRangeException)
                {
                    errorReporter.ReportError(nodePair.Value, $"'{rawAzureCloud}' is not a supported value for 'cloud'.");
                }
            }
            else
            {
                errorReporter.ReportError(nodePair.Value, $"'{rawAzureCloud}' is not a valid value for 'cloud'.");
            }

            return null;
        }

        private object DeserializeEndpoints(string rawEndpoints, KeyValuePair<YamlNode, YamlNode> nodePair, IErrorReporter errorReporter)
        {

            if (_azureCloud == AzureCloud.Custom)
            {
                return _azureEndpointsDeserializer.Deserialize((YamlMappingNode)nodePair.Value, errorReporter);
            }

            return null;
        }
    }
}
