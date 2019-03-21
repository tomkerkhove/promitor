using System;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class InvalidMetricException : Exception
    {
        public InvalidMetricException(string metricName) : base($"Invalid metric name '{metricName}' for storage queue")
        {
        }
    }
}