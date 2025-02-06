using Azure.Monitor.Query;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Core.Extensions;
using System;

namespace Promitor.Agents.ResourceDiscovery.Extensions
{
    public static class AzureLandscapeExtensions
    {
        public static AzureEnvironment GetAzureEnvironment(this AzureLandscape azureLandscape) => azureLandscape.Cloud.GetAzureEnvironment(azureLandscape.Endpoints);

        public static MetricsQueryAudience DetermineMetricsClientAudience(this AzureLandscape azureLandscape) => azureLandscape.Cloud.DetermineMetricsClientAudience(azureLandscape.Endpoints);

        public static MetricsClientAudience DetermineMetricsClientBatchQueryAudience(this AzureLandscape azureLandscape) => azureLandscape.Cloud.DetermineMetricsClientBatchQueryAudience(azureLandscape.Endpoints);

        public static Uri GetAzureAuthorityHost(this AzureLandscape azureLandscape) => azureLandscape.Cloud.GetAzureAuthorityHost(azureLandscape.Endpoints);
    }
}
