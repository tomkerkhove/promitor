using System.Collections.Generic;
using GuardNet;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    /// <summary>
    /// Contains the details of the prometheus metric that will be created.
    /// </summary>
    public class PrometheusMetricDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrometheusMetricDefinition"/> class.
        /// </summary>
        /// <param name="name">Name of the metric to use when exposing in the scraping endpoint.</param>
        /// <param name="description">Description concerning metric that will be made available in the scraping endpoint.</param>
        /// <param name="labels">Collection of custom labels to add to every metric.</param>
        public PrometheusMetricDefinition(string name, string description, Dictionary<string, string> labels)
        {
            Guard.NotNull(labels, nameof(labels));

            Name = name;
            Description = description;
            Labels = labels;
        }

        /// <summary>
        ///     Name of the metric to use when exposing in the scraping endpoint
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Description concerning metric that will be made available in the scraping endpoint
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     Collection of custom labels to add to every metric
        /// </summary>
        public Dictionary<string, string> Labels { get; }
    }
}
