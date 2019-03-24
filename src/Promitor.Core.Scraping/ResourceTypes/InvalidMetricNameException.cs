using System;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class InvalidMetricNameException : Exception
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="metricName">Name of the metric that was not valid</param>
        /// <param name="resourceProviderName">Name of the resource provider that was configured, ie. Service Bus Queues</param>
        public InvalidMetricNameException(string metricName, string resourceProviderName) : base($"Invalid metric name '{metricName}' for {resourceProviderName}")
        {
            MetricName = metricName;
            ResourceProviderName = resourceProviderName;
        }

        /// <summary>
        ///     Name of the metric that was not valid
        /// </summary>
        public string MetricName { get; }

        /// <summary>
        ///     Name of the resource provider that was configured
        /// </summary>
        public string ResourceProviderName { get; }
    }
}