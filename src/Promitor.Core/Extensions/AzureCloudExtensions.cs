﻿using System;
using Azure.Identity;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Promitor.Core.Serialization.Enum;

namespace Promitor.Core.Extensions
{
    public static class AzureCloudExtensions
    {
        /// <summary>
        ///     Get Azure environment information
        /// </summary>
        /// <param name="azureCloud">Microsoft Azure cloud</param>
        /// <returns>Azure environment information for specified cloud</returns>
        public static AzureEnvironment GetAzureEnvironment(this AzureCloud azureCloud)
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(azureCloud), "No Azure environment is known for");
            }
        }

        public static Uri GetAzureAuthorityHost(this AzureCloud azureCloud)
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(azureCloud), "No Azure environment is known for");
            }
        }
    }
}