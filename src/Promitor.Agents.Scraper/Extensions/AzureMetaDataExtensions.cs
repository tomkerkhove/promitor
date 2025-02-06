using Azure.Monitor.Query;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Promitor.Core.Extensions;
using Promitor.Core.Scraping.Configuration.Model;
using System;

namespace Promitor.Agents.Scraper.Extensions
{
    public static class AzureMetaDataExtensions
    {
        public static AzureEnvironment GetAzureEnvironment(this AzureMetadata azureMetadata) => azureMetadata.Cloud.GetAzureEnvironment(azureMetadata.Endpoints);

        public static MetricsQueryAudience DetermineMetricsClientAudience(this AzureMetadata azureMetadata) => azureMetadata.Cloud.DetermineMetricsClientAudience(azureMetadata.Endpoints);

        public static MetricsClientAudience DetermineMetricsClientBatchQueryAudience(this AzureMetadata azureMetadata) => azureMetadata.Cloud.DetermineMetricsClientBatchQueryAudience(azureMetadata.Endpoints);

        public static Uri GetAzureAuthorityHost(this AzureMetadata azureMetadata) => azureMetadata.Cloud.GetAzureAuthorityHost(azureMetadata.Endpoints);
    }
}
