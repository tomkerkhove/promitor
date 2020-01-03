using System;
using System.Linq;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;

namespace Promitor.Integrations.AzureMonitor
{
    public class MeasuredMetric
    {
        /// <summary>
        ///     Value of the metric that was found
        /// </summary>
        public double? Value { get; }

        /// <summary>
        ///     Name of dimension for a metric
        /// </summary>
        public string DimensionName { get; }

        /// <summary>
        ///     Name of dimension for a metric
        /// </summary>
        public string DimensionValue { get; }

        /// <summary>
        ///     Indication whether or not the metric represents a dimension
        /// </summary>
        public bool IsDimensional { get; }

        private MeasuredMetric(double? value)
        {
            Value = value;
        }

        private MeasuredMetric(double? value, string dimensionName, string dimensionValue)
        {
            Guard.NotNullOrWhitespace(dimensionName, nameof(dimensionName));
            Guard.NotNullOrWhitespace(dimensionValue, nameof(dimensionValue));

            Value = value;

            IsDimensional = true;
            DimensionName = dimensionName;
            DimensionValue = dimensionValue;
        }

        /// <summary>
        /// Create a measured metric without dimension
        /// </summary>
        /// <param name="value">Measured metric value</param>
        public static MeasuredMetric CreateWithoutDimension(double? value)
        {
            return new MeasuredMetric(value);
        }

        /// <summary>
        /// Create a measured metric for a given dimension
        /// </summary>
        /// <param name="value">Measured metric value</param>
        /// <param name="dimensionName">Name of dimension that is being scraped</param>
        /// <param name="timeseries">Timeseries representing one of the dimensions</param>
        internal static MeasuredMetric CreateForDimension(double? value, string dimensionName, TimeSeriesElement timeseries)
        {
            Guard.NotNullOrWhitespace(dimensionName, nameof(dimensionName));
            Guard.NotNull(timeseries, nameof(timeseries));
            Guard.For<ArgumentException>(() => timeseries.Metadatavalues.Any() == false);

            var dimensionValue = timeseries.Metadatavalues.Single(metadataValue => metadataValue.Name?.Value.Equals(dimensionName, StringComparison.InvariantCultureIgnoreCase) == true);
            return new MeasuredMetric(value, dimensionName, dimensionValue.Value);
        }
    }
}
