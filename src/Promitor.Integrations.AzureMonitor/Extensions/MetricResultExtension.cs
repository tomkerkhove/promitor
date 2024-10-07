using Azure.Monitor.Query.Models;

namespace Promitor.Integrations.AzureMonitor.Extensions
{
    public static class MetricResultExtension 
    {   
        /// <summary>
        ///     hacky to to get resource ID since it's not available directly through the SDK model, because the MetricResult model does not have the ResourceID attribute that comes with Response JSON 
        /// </summary>
        public static string ParseResourceIdFromResultId(this MetricResult metricResult) 
        {
            return ExtractResourceId(metricResult.Id);
        }

        private static string ExtractResourceId(string fullId)
        {
            // Find the index of the second occurrence of "/providers/"
            int firstIndex = fullId.IndexOf("/providers/", System.StringComparison.Ordinal);
            int secondIndex = fullId.IndexOf("/providers/", firstIndex + 1, System.StringComparison.Ordinal);

            // If the second "/providers/" is found, slice the string up to that point
            if (secondIndex != -1)
            {
                return fullId[..secondIndex];
            }

            // If not found, return the full string
            return fullId;
        }
    }
}