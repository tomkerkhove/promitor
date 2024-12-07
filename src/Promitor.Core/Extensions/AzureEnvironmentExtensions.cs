using Humanizer;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using System;

namespace Promitor.Core.Extensions
{
    public static class AzureEnvironmentExtensions
    {
        /// <summary>
        ///     Get Azure environment information
        /// </summary>
        /// <param name="azureCloud">Microsoft Azure cloud</param>
        /// <returns>Azure environment information for specified cloud</returns>
        public static string GetDisplayName(this AzureEnvironment azureCloud)
        {
            return azureCloud.Name.Replace("Azure", "").Replace("Cloud", "").Humanize(LetterCasing.Title);
        }

        public static AzureEnvironment AzureCustomCloud = new AzureEnvironment()
        {
            Name = nameof(AzureCustomCloud),
            AuthenticationEndpoint = Environment.GetEnvironmentVariable("PROMITOR_AUTH_ENDPOINT"),
            ResourceManagerEndpoint = Environment.GetEnvironmentVariable("PROMITOR_RESOURCE_MANAGER_ENDPOINT"),
            ManagementEndpoint = Environment.GetEnvironmentVariable("PROMITOR_MANAGEMENT_ENDPOINT"),
            GraphEndpoint = Environment.GetEnvironmentVariable("PROMITOR_GRAPH_ENDPOINT"),
            StorageEndpointSuffix = Environment.GetEnvironmentVariable("PROMITOR_STORAGE_ENDPOINT_SUFFIX"),
            KeyVaultSuffix = Environment.GetEnvironmentVariable("PROMITOR_KEY_VAULT_SUFFIX")
        };
    }
}