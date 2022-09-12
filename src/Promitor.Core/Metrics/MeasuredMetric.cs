using System;
using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;

namespace Promitor.Core.Metrics
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
        public List<string> DimensionNames { get; set; }

        /// <summary>
        ///     Name of dimension for a metric
        /// </summary>
        public List<string> DimensionValues { get; }

        /// <summary>
        ///     Indication whether or not the metric represents a dimension
        /// </summary>
        public bool IsDimensional { get; }

        private MeasuredMetric(double? value)
        {
            Value = value;
        }

        private MeasuredMetric(double? value, List<string> dimensionNames, List<string> dimensionValues)
        {
            Guard.NotAny(dimensionNames, nameof(dimensionNames));
            Guard.NotAny(dimensionValues, nameof(dimensionValues));

            Value = value;

            IsDimensional = true;
            DimensionNames = dimensionNames;
            DimensionValues = dimensionValues;
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
        /// <param name="dimensionNames">List of names of dimensions that are being scraped</param>
        /// <param name="timeseries">Timeseries representing one of the dimensions</param>
        public static MeasuredMetric CreateForDimension(double? value, List<string> dimensionNames, TimeSeriesElement timeseries)
        {
            Guard.NotAny(dimensionNames, nameof(dimensionNames));
            Guard.NotNull(timeseries, nameof(timeseries));
            Guard.For<ArgumentException>(() => timeseries.Metadatavalues.Any() == false);

            var dimensionValues = new List<string>(); 
            foreach (var dimensionName in dimensionNames)
            {
                dimensionValues.Add(timeseries.Metadatavalues.Single(metadataValue => metadataValue.Name?.Value.Equals(dimensionName, StringComparison.InvariantCultureIgnoreCase) == true).Value);
            }
            
            return CreateForDimension(value, dimensionNames, dimensionValues);
        }

        /// <summary>
        /// Create a measured metric for a given dimension
        /// </summary>
        /// <param name="value">Measured metric value</param>
        /// <param name="dimensionNames">List of names of dimensions that are being scraped</param>
        /// <param name="dimensionValues">List of values of the dimension that are being scraped</param>
        public static MeasuredMetric CreateForDimension(double? value, List<string> dimensionNames, List<string> dimensionValues)
        {
            Guard.NotAny(dimensionNames, nameof(dimensionNames));
            Guard.NotAny(dimensionValues, nameof(dimensionValues));

            return new MeasuredMetric(value, dimensionNames, dimensionValues);
        }
    }
}
