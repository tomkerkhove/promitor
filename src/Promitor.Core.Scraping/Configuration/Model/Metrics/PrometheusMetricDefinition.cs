using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    public class PrometheusMetricDefinition
    {
        public PrometheusMetricDefinition(string name, string description, Dictionary<string, string> labels)
        {
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
