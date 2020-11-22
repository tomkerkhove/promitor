using System;
using GuardNet;

namespace Promitor.Integrations.AzureMonitor.Exceptions
{
    public class MetricInformationNotFoundException : Exception
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="name">Name of the metric</param>
        /// <param name="details">Details that provide more context about the scenario</param>
        public MetricInformationNotFoundException(string name, string details) : base($"No metric information was found for '{name}'. Reason: '{details}'")
        {
            Guard.NotNullOrWhitespace(name, nameof(name));
            Guard.NotNullOrWhitespace(details, nameof(details));

            Name = name;
            Details = details;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="name">Name of the metric</param>
        /// <param name="dimension">Dimension of the metric</param>
        /// <param name="details">Details that provide more context about the scenario</param>
        public MetricInformationNotFoundException(string name, string details, string dimension) : base($"No metric information was found for '{name}' with dimension '{dimension}'. Reason: '{details}'")
        {
            Guard.NotNullOrWhitespace(name, nameof(name));
            Guard.NotNullOrWhitespace(details, nameof(details));

            Name = name;
            Dimension = dimension;
            Details = details;
        }

        /// <summary>
        ///     Name of the metric
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Dimension of the metric
        /// </summary>
        public string Dimension { get; }

        /// <summary>
        ///     Details that provide more context about the scenario
        /// </summary>
        public string Details { get; }
    }
}