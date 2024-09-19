using System;
using System.Text.RegularExpressions;
using Azure.Monitor.Query.Models;

namespace Promitor.Integrations.AzureMonitor.Extensions
{
    public static class MetricResultExtension 
    {   
        // hacky to to get resource ID since it's not available directly through the SDK model
        static string resourceIdPattern = @"^([\/]?subscriptions\/[^\/]+\/resourceGroups\/[^\/]+\/providers\/[^\/]+\/[^\/]+\/[^\/]+)";     
        static Regex resourceIdRegex = new Regex(resourceIdPattern, RegexOptions.Compiled);
        
        public static string ParseResourceIdFromResultId(this MetricResult metricResult) 
        {
            // Match match = resourceIdRegex.Match(metricResult.Id);
            // if (!match.Success || string.IsNullOrEmpty(match.Groups[1].Value))
            // {
            //     throw new InvalidOperationException($"The expected resource ID pattern was not found in the input string {metricResult.Id}");
            // }

            // string resourceId = match.Groups[1].Value;
            // return resourceId;
            return ExtractResourceId(metricResult.Id);
        }

        private static string ExtractResourceId(string fullId)
        {
            // Find the index of the second occurrence of "/providers/"
            int firstIndex = fullId.IndexOf("/providers/");
            int secondIndex = fullId.IndexOf("/providers/", firstIndex + 1);

            // If the second "/providers/" is found, slice the string up to that point
            if (secondIndex != -1)
            {
                return fullId.Substring(0, secondIndex);
            }

            // If not found, return the full string
            return fullId;
        }

    }
}