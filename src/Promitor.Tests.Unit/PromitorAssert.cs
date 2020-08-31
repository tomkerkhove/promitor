using Microsoft.Azure.Management.ResourceManager.Fluent;
using Promitor.Agents.Core.Validation;
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

        /// <summary>
        ///     Verifies that validation was successful
        /// </summary>
        public static void ValidationIsSuccessful(ValidationResult validationResult)
        {
            True(validationResult.IsSuccessful, $"Validation was not successful. Reason: {validationResult.Message}");
        }

        /// <summary>
        ///     Verifies that validation failed
        /// </summary>
        public static void ValidationFailed(ValidationResult validationResult)
        {
            False(validationResult.IsSuccessful, "Validation was successful");
        }
    }
}