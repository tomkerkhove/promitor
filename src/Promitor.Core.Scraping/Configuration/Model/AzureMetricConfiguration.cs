using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Promitor.Core.Scraping.Configuration.Model
{
    public class AzureMetricConfiguration
    {
        /// <summary>
        ///     Name of the Azure Monitor metric to query
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        ///     Amount of maximum resources to limit the results to
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        ///     Information about the dimensions of an Azure Monitor metric
        /// </summary>
        public IReadOnlyCollection<MetricDimension> Dimensions { get; set; }

        /// <summary>
        ///     Information about the dimension of an Azure Monitor metric
        /// </summary>
        [Obsolete("Dimension is deprecated, please use Dimensions instead.")]
        public MetricDimension Dimension { get; set; }

        /// <summary>
        ///     Configuration on how to aggregate the metric
        /// </summary>
        public MetricAggregation Aggregation { get; set; }

        /// <summary>
        ///    Checks whether the configuration contains a dimension with the given name.
        /// </summary>
        /// <param name="dimensionName">Dimension name to be checked for.</param>
        /// <returns>true if the dimension name was found, false otherwise</returns>
        public bool? HasDimension(string dimensionName)
        {
            if (Dimension != null)
            {
                return Dimension?.Name?.Equals(dimensionName, StringComparison.InvariantCultureIgnoreCase);
            }
            return Dimensions?.Any(dimension => dimension.Name.Equals(dimensionName, StringComparison.InvariantCultureIgnoreCase));
        }

         // A unique string to represent this Azure metric and its configured dimensions
        public string ToUniqueStringRepresentation()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MetricName);
            if (Dimension != null) 
            {
                sb.Append('_');
                sb.Append(Dimension.Name);
            }
            else if (Dimensions != null) 
            {
                foreach (var dimension in Dimensions)
                {
                    sb.Append('_');
                    sb.Append(dimension.Name);
                }
            }
            sb.Append($"_limit{Limit}");

            if (Aggregation != null)
            {
                sb.Append("_agg");
                sb.Append(Aggregation.Type.ToString());

                if (Aggregation.Interval.HasValue)
                {
                    sb.Append("_interval");
                    sb.Append(Aggregation.Interval.Value.ToString("c"));
                }
            }

            return sb.ToString();
        }
    }
}