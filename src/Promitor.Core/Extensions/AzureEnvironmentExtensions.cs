using Humanizer;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Promitor.Core.Configuration;
using Promitor.Core.Serialization.Enum;

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

        /// <summary>
        ///    Get Azure environment information for custom cloud
        /// </summary>
        /// <param name="azureCloud"></param>
        /// <param name="endpoints"></param>
        /// <returns></returns>
        public static AzureEnvironment GetCustomAzureEnvironment(AzureCloud azureCloud, AzureEndpoints endpoints)
        {
            return new AzureEnvironment
            {
                Name = nameof(AzureCloud.Custom),
                AuthenticationEndpoint = endpoints.AuthenticationEndpoint,
                ResourceManagerEndpoint = endpoints.ResourceManagerEndpoint,
                GraphEndpoint = endpoints.GraphEndpoint,
                ManagementEndpoint = endpoints.ManagementEndpoint,
                StorageEndpointSuffix = endpoints.StorageEndpointSuffix,
                KeyVaultSuffix = endpoints.KeyVaultSuffix,
            };
        }
    }
}