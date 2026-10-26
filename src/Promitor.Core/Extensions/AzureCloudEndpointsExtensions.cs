using System;
using Azure.Identity;
using Azure.Monitor.Query;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Promitor.Core.Serialization.Enum;

namespace Promitor.Core.Extensions
{
    public static class AzureCloudEndpointsExtensions
    {
        /// <summary>
        ///     Get Azure environment information under legacy SDK model
        /// </summary>
        /// <param name="cloudEndpoints">Azure Cloud and endpoints information.</param>
        /// <returns>Azure environment information for specified cloud</returns>
        public static AzureEnvironment GetAzureEnvironment(this IAzureCloudEndpoints cloudEndpoints)
        {
            switch (cloudEndpoints.Cloud)
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
                    return AzureEnvironmentExtensions.GetCustomAzureEnvironment(cloudEndpoints.Endpoints);
                default:
                    throw new ArgumentOutOfRangeException(nameof(cloudEndpoints.Cloud), "No Azure environment is known for in legacy SDK");
            }
        }

        /// <summary>
        ///     Get Azure environment information for Azure.Monitor SDK single resource queries
        /// </summary>
        /// <param name="cloudEndpoints">Azure Cloud and endpoints information.</param>
        /// <returns>Azure environment information for specified cloud</returns>
        public static MetricsQueryAudience DetermineMetricsClientAudience(this IAzureCloudEndpoints cloudEndpoints)
        {
            switch (cloudEndpoints.Cloud)
            {
                case AzureCloud.Global:
                    return MetricsQueryAudience.AzurePublicCloud;
                case AzureCloud.UsGov:
                    return MetricsQueryAudience.AzureGovernment;
                case AzureCloud.China:
                    return MetricsQueryAudience.AzureChina;
                case AzureCloud.Custom:
                    return new MetricsQueryAudience(cloudEndpoints.Endpoints.MetricsQueryAudience);
                default:
                    throw new ArgumentOutOfRangeException(nameof(cloudEndpoints.Cloud), "No Azure environment is known for");
            }
        }

        /// <summary>
        ///     Get Azure environment information for Azure.Monitor SDK batch queries
        /// </summary>
        /// <param name="cloudEndpoints">Cloud endpoints describing the configuration.</param>
        /// <returns>Azure environment information for specified cloud</returns>
        public static MetricsClientAudience DetermineMetricsClientBatchQueryAudience(this IAzureCloudEndpoints cloudEndpoints)
        {
            switch (cloudEndpoints.Cloud)
            {
                case AzureCloud.Global:
                    return MetricsClientAudience.AzurePublicCloud;
                case AzureCloud.UsGov:
                    return MetricsClientAudience.AzureGovernment;
                case AzureCloud.China:
                    return MetricsClientAudience.AzureChina;
                case AzureCloud.Custom:
                    return new MetricsClientAudience(cloudEndpoints.Endpoints.MetricsClientAudience);
                default:
                    throw new ArgumentOutOfRangeException(nameof(cloudEndpoints.Cloud), "No Azure environment is known for");
            }
        }

        /// <summary>
        ///     Get the Azure authority host URI for the specified cloud endpoints.
        /// </summary>
        /// <param name="cloudEndpoints">Cloud endpoints describing the configuration.</param>
        /// <returns>Uri that represents the authority host.</returns>
        public static Uri GetAzureAuthorityHost(this IAzureCloudEndpoints cloudEndpoints)
        {
            switch (cloudEndpoints.Cloud)
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
                    return new Uri(cloudEndpoints.Endpoints.AuthenticationEndpoint);
                default:
                    throw new ArgumentOutOfRangeException(nameof(cloudEndpoints.Cloud), "No Azure environment is known for");
            }
        }

        public static Uri GetLogAnalyticsEndpoint(this IAzureCloudEndpoints cloudEndpoints)
        {
            switch(cloudEndpoints.Cloud)
            {
                case AzureCloud.Global:
                    return new Uri("https://api.loganalytics.io");
                case AzureCloud.UsGov:
                    return new Uri("https://api.loganalytics.us");
                case AzureCloud.Custom:
                    return new Uri(cloudEndpoints.Endpoints.LogAnalyticsEndpoint);
                default:
                    throw new NotSupportedException($"Environment {cloudEndpoints.Cloud} is not supported for scraping Azure Log Analytics resource(s)");
            }
        }
    }
}