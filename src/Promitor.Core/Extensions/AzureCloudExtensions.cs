using System;
using Promitor.Core.Serialization.Enum;

namespace Promitor.Core.Extensions
{
    public static class AzureCloudExtensions
    {
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