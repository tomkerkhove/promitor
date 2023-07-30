using System;
using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Metrics.Exceptions;

namespace Promitor.Core.Metrics
{
    public class MeasuredMetric
    {
        /// <summary>
        ///     Value of the metric that was found
        /// </summary>
        public double? Value { get; }

        /// <summary>
        ///     Measured dimensions.
        /// </summary>
        public List<MeasuredMetricDimension> Dimensions { get; }

        /// <summary>
        ///     Indication whether or not the metric represents a dimension
        /// </summary>
        public bool IsDimensional { get; }

        private MeasuredMetric(double? value)
        {
            Value = value;
        }

        private MeasuredMetric(double? value, List<MeasuredMetricDimension> dimensions)
        {
            Guard.NotAny(dimensions, nameof(dimensions));

            Value = value;

            IsDimensional = true;
            Dimensions = dimensions;
        }

        /// <summary>
        /// Create a measured metric without dimensions
        /// </summary>
        /// <param name="value">Measured metric value</param>
        public static MeasuredMetric CreateWithoutDimensions(double? value)
        {
            return new MeasuredMetric(value);
        }

        /// <summary>
        /// Create a measured metric for given dimensions
        /// </summary>
        /// <param name="value">Measured metric value</param>
        /// <param name="dimensionNames">List of names of dimensions that are being scraped</param>
        /// <param name="timeseries">Timeseries representing one of the dimensions</param>
        public static MeasuredMetric CreateForDimensions(double? value, List<string> dimensionNames, TimeSeriesElement timeseries)
        {
            Guard.NotAny(dimensionNames, nameof(dimensionNames));
            Guard.NotNull(timeseries, nameof(timeseries));
            
            var dimensions = new List<MeasuredMetricDimension>();
            foreach (var dimensionName in dimensionNames)
            {
                var dimensionMetadataValue = timeseries.Metadatavalues.Where(metadataValue => metadataValue.Name?.Value.Equals(dimensionName, StringComparison.InvariantCultureIgnoreCase) == true).ToList();            
                if(!dimensionMetadataValue.Any())
                {
                    throw new MissingDimensionException(dimensionName, timeseries);
                }
                var dimensionValue = dimensionMetadataValue.Single().Value;
                dimensions.Add(new MeasuredMetricDimension(dimensionName, dimensionValue));
            }

            return new MeasuredMetric(value, dimensions);
        }

        /// <summary>
        /// Create a measured metric for given dimensions when no metric information was found
        /// </summary>
        /// <param name="dimensionNames">List of names of dimensions that are being scraped</param>
        public static MeasuredMetric CreateForDimensions(List<string> dimensionNames)
        {
            Guard.NotAny(dimensionNames, nameof(dimensionNames));

            var dimensions = dimensionNames.Select(name => new MeasuredMetricDimension(name, "unknown")).ToList();

            return new MeasuredMetric(null, dimensions);
        }
    }
}
