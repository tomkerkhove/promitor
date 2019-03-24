using System;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class InvalidMetricNameException : Exception
    {
        public string MetricName { get; }
        public string ResourceProviderName { get; }

        public InvalidMetricNameException(string metricName, string resourceProviderName) : base($"Invalid metric name '{metricName}' for {resourceProviderName}")
        {
            MetricName = metricName;
            ResourceProviderName = resourceProviderName;
        }
    }
}