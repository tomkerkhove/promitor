using System;
using Azure.Identity;
using Azure.Monitor.Query;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Promitor.Core.Configuration;
using Promitor.Core.Serialization.Enum;

namespace Promitor.Core.Extensions
{
    public static class AzureCloudExtensions
    {
        /// <summary>
        ///     Get Azure environment information under legacy SDK model
        /// </summary>
        /// <param name="azureCloud">Microsoft Azure cloud</param>
        /// <param name="endpoints">Azure endpoints</param>
        /// <returns>Azure environment information for specified cloud</returns>
        public static AzureEnvironment GetAzureEnvironment(this AzureCloud azureCloud, AzureEndpoints endpoints)
        {
            switch (azureCloud)
            {
                case AzureCloud.Global:
                    return AzureEnvironment.AzureGlobalCloud;
                case AzureCloud.China:
                    return AzureEnvironment.AzureChinaCloud;
                case AzureCloud.Germany:
                    return AzureEnvironment.AzureGermanCloud;
                case AzureCloud.UsGov:
                    return AzureEnvironment.AzureUSGovernment;
                case AzureCloud.Custom:
                    return AzureEnvironmentExtensions.GetCustomAzureEnvironment(azureCloud, endpoints);
                default:
                    throw new ArgumentOutOfRangeException(nameof(azureCloud), "No Azure environment is known for in legacy SDK");
            }
        }

        /// <summary>
        ///     Get Azure environment information for Azure.Monitor SDK single resource queries
        /// </summary>
        /// <param name="azureCloud">Microsoft Azure cloud</param>
        /// <param name="endpoints">Azure endpoints</param>
        /// <returns>Azure environment information for specified cloud</returns>
        public static MetricsQueryAudience DetermineMetricsClientAudience(this AzureCloud azureCloud, AzureEndpoints endpoints) {
            switch (azureCloud) 
            {   
                case AzureCloud.Global:
                    return MetricsQueryAudience.AzurePublicCloud;
                case AzureCloud.UsGov:
                    return MetricsQueryAudience.AzureGovernment;
                case AzureCloud.China:
                    return MetricsQueryAudience.AzureChina;
                case AzureCloud.Custom:
                    return new MetricsQueryAudience(endpoints.MetricsQueryAudience);
                default:
                    throw new ArgumentOutOfRangeException(nameof(azureCloud), "No Azure environment is known for"); // Azure.Monitory.Query package does not support any other sovereign regions
            }
        }

        /// <summary>
        ///     Get Azure environment information for Azure.Monitor SDK batch queries
        /// </summary>
        /// <param name="azureCloud">Microsoft Azure cloud</param>
        /// <param name="endpoints">Azure endpoints</param>
        /// <returns>Azure environment information for specified cloud</returns>
        public static MetricsClientAudience DetermineMetricsClientBatchQueryAudience(this AzureCloud azureCloud, AzureEndpoints endpoints) {
            switch (azureCloud) 
            {   
                case AzureCloud.Global:
                    return MetricsClientAudience.AzurePublicCloud;
                case AzureCloud.UsGov:
                    return MetricsClientAudience.AzureGovernment;
                case AzureCloud.China:
                    return MetricsClientAudience.AzureChina;
                case AzureCloud.Custom:
                    return new MetricsClientAudience(endpoints.MetricsClientAudience);
                default:
                    throw new ArgumentOutOfRangeException(nameof(azureCloud), "No Azure environment is known for"); // Azure.Monitory.Query package does not support any other sovereign regions
            }
        }

        /// <summary>
        ///    Get well known authority hosts for the Azure cloud
        /// </summary>
        /// <param name="azureCloud"></param>
        /// <param name="endpoints"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Uri GetAzureAuthorityHost(this AzureCloud azureCloud, AzureEndpoints endpoints)
        {
            switch (azureCloud)
            {
                case AzureCloud.Global:
                    return AzureAuthorityHosts.AzurePublicCloud;
                case AzureCloud.China:
                    return AzureAuthorityHosts.AzureChina;
                case AzureCloud.Germany:
                    return AzureAuthorityHosts.AzureGermany;
                case AzureCloud.UsGov:
                    return AzureAuthorityHosts.AzureGovernment;
                case AzureCloud.Custom:
                    return new Uri(endpoints.AuthenticationEndpoint);
                default:
                    throw new ArgumentOutOfRangeException(nameof(azureCloud), "No Azure environment is known for");
            }
        }

        /// <summary>
        ///    Validates if Azure cloud is supported for metric scraping by the Azure Monitor SDK.
        /// </summary>
        /// <param name="azureCloud"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ValidateMetricsClientAudience(this AzureCloud azureCloud)
        {
            switch (azureCloud)
            {
                case AzureCloud.Global:
                case AzureCloud.China:
                case AzureCloud.UsGov:
                case AzureCloud.Custom:
                    // These are supported, do nothing
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(azureCloud), "No Azure environment is known for"); // Azure.Monitory.Query package does not support any other sovereign regions
            }
        }
    }
}