using System;

namespace Promitor.Integrations.AzureMonitor.Exceptions
{
    public class MetricInformationNotFoundException : Exception
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="metricName">Name of the metric</param>
        /// <param name="details">Details that provide more context about the scenario</param>
        public MetricInformationNotFoundException(string metricName, string details) : base($"No metric information was found for '{metricName}'. Reason: '{details}'")
        {
            Guard.Guard.NotNullOrWhitespace(metricName, nameof(metricName));
            Guard.Guard.NotNullOrWhitespace(details, nameof(details));

            MetricName = metricName;
            Details = details;
        }

        /// <summary>
        ///     Name of the metric
        /// </summary>
        public string MetricName { get; }

        /// <summary>
        ///     Details that provide more context about the scenario
        /// </summary>
        public string Details { get; }
    }
}