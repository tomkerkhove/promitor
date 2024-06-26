﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Promitor.Core.Extensions;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Serialization.Enum;
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
                .WithDefault(AzureCloud.Global)
                .MapUsing(DetermineAzureCloud);
        }
        private object DetermineAzureCloud(string rawAzureCloud, KeyValuePair<YamlNode, YamlNode> nodePair, IErrorReporter errorReporter)
        {
            if (Enum.TryParse<AzureCloud>(rawAzureCloud, out var azureCloud))
            {
                try
                {
                    azureCloud.DetermineMetricsClientAudience();
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
    }
}
