using Humanizer;
using Microsoft.Azure.Management.ResourceManager.Fluent;

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
    }
}