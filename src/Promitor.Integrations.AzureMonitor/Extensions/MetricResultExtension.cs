using System;
using System.Text.RegularExpressions;
using Azure.Monitor.Query.Models;

namespace Promitor.Integrations.AzureMonitor.Extensions
{
    public static class MetricResultExtension 
    {   
        // hacky to to get resource ID since it's not available directly through the SDK model
        static string resourceIdPattern = @"^(/subscriptions/[^/]+/resourceGroups/[^/]+/providers/[^/]+/[^/]+/[^/]+)";     
        static Regex resourceIdRegex = new Regex(resourceIdPattern, RegexOptions.Compiled);
        
        public static string ParseResourceIdFromResultId(this MetricResult metricResult) 
        {
            Match match = resourceIdRegex.Match(metricResult.Id);
            if (!match.Success || string.IsNullOrEmpty(match.Groups[1].Value))
            {
                throw new InvalidOperationException("The expected resource ID pattern was not found in the input string.");
            }

            string resourceId = match.Groups[1].Value;
            return resourceId;
        }

    }
}