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

        public bool Equals(MetricDimension other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(other.Name, Name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MetricDimension);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode - this is providing the means to compare MetricDimension instances without having consumers become dependent on class members
            return Name?.ToLowerInvariant().GetHashCode() ?? 0;
        }
    }
}