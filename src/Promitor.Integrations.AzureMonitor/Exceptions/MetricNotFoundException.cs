using System;

namespace Promitor.Integrations.AzureMonitor.Exceptions
{
    using Guard;
    
    public class MetricNotFoundException : Exception
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="metricName">Name of the metric</param>
        public MetricNotFoundException(string metricName) : base($"The metric '{metricName}' was not found")
        {
            Guard.NotNullOrWhitespace(metricName, nameof(metricName));

            MetricName = metricName;
        }

        /// <summary>
        ///     Name of the metric
        /// </summary>
        public string MetricName { get; }
    }
}