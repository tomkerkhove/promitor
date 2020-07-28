using Microsoft.Azure.Management.ResourceManager.Fluent;
using Xunit;

namespace Promitor.Tests.Unit
{
    internal class PromitorAssert : Assert
    {
        /// <summary>
        ///     Verifies if Azure environment information are equal
        /// </summary>
        public static void ContainsSameAzureEnvironmentInfo(AzureEnvironment expectedEnvironment, AzureEnvironment azureEnvironment)
        {
            NotNull(azureEnvironment);
            Equal(expectedEnvironment.Name, azureEnvironment.Name);
            Equal(expectedEnvironment.AuthenticationEndpoint, azureEnvironment.AuthenticationEndpoint);
            Equal(expectedEnvironment.GraphEndpoint, azureEnvironment.GraphEndpoint);
            Equal(expectedEnvironment.KeyVaultSuffix, azureEnvironment.KeyVaultSuffix);
            Equal(expectedEnvironment.ManagementEndpoint, azureEnvironment.ManagementEndpoint);
            Equal(expectedEnvironment.ResourceManagerEndpoint, azureEnvironment.ResourceManagerEndpoint);
            Equal(expectedEnvironment.StorageEndpointSuffix, azureEnvironment.StorageEndpointSuffix);
        }
    }
}