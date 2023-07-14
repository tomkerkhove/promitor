using System;
using GuardNet;

namespace Promitor.Core.Metrics.Exceptions
{
    public class MissingDimensionException : Exception
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="metricName">Name of the dimension</param>
        public MissingDimensionException(string dimensionName) : base($"No value found for dimension '{dimensionName}'")
        {
            Guard.NotNullOrWhitespace(dimensionName, nameof(dimensionName));

            DimensionName = dimensionName;
        }

        /// <summary>
        ///     Name of the dimension
        /// </summary>
        public string DimensionName { get; }
    }
}