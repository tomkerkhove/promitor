using System;
using System.Collections.Generic;
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
        /// <param name="details">Details that provide more context about the scenario</param>
        /// <param name="dimensions">Dimensions of the metric</param>
        public MetricInformationNotFoundException(string name, string details, List<string> dimensions) : base($"No metric information was found for '{name}' with dimensions '{dimensions}'. Reason: '{details}'")
        {
            Guard.NotNullOrWhitespace(name, nameof(name));
            Guard.NotNullOrWhitespace(details, nameof(details));

            Name = name;
            Dimensions = dimensions;
            Details = details;
        }

        /// <summary>
        ///     Name of the metric
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Dimension of the metric
        /// </summary>
        public List<string> Dimensions { get; }

        /// <summary>
        ///     Details that provide more context about the scenario
        /// </summary>
        public string Details { get; }
    }
}