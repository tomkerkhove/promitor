using System;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;

namespace Promitor.Core.Metrics.Exceptions
{
    public class MissingDimensionException : Exception
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="dimensionName">Name of the dimension</param>
        /// <param name="timeSeries">Time series element missing the dimension</param>
        public MissingDimensionException(string dimensionName, TimeSeriesElement timeSeries) : base($"No value found for dimension '{dimensionName}'")
        {
            Guard.NotNullOrWhitespace(dimensionName, nameof(dimensionName));

            DimensionName = dimensionName;
            TimeSeries = timeSeries; 
        }

        /// <summary>
        ///     Name of the dimension
        /// </summary>
        public string DimensionName { get; }

        /// <summary>
        ///     Time series element producing the exception  
        /// </summary>
        public TimeSeriesElement TimeSeries { get; }
    }
}