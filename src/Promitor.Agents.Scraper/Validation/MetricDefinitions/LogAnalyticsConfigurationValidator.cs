using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions
{
    public class LogAnalyticsConfigurationValidator
    {
        public IEnumerable<string> Validate(LogAnalyticsConfiguration logAnalyticsConfiguration)
        {
            var errorMessages = new List<string>();

            if (logAnalyticsConfiguration == null)
            {
                errorMessages.Add("Invalid Log Analytics is configured");
                return errorMessages;
            }

            if (string.IsNullOrWhiteSpace(logAnalyticsConfiguration.Query))
            {
                errorMessages.Add("No Query for Log Analytics is configured");
            }

            if (logAnalyticsConfiguration.LogAnalyticsAggregation?.Interval == null)
            {
                errorMessages.Add("No Log Analytics Interval is configured");
            }

            return errorMessages;
        }
    }
}