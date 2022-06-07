using System;

namespace Promitor.Core.Scraping.Configuration.Model
{
    /// <summary>
    /// Information about the dimension of an Azure Monitor metric
    /// </summary>
    public class MetricDimension : IEquatable<MetricDimension>
    {
        /// <summary>
        ///     Name of the dimension
        /// </summary>
        public string Name { get; set; }


        #region Equality

        public bool Equals(MetricDimension other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return string.Equals(other.Name, Name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MetricDimension);
        }

        public override int GetHashCode()
        {
            return Name?.ToLowerInvariant().GetHashCode() ?? 0;
        }
        
        #endregion
    }
}