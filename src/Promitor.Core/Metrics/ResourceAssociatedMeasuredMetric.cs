using System.Collections.Generic;

namespace Promitor.Core.Metrics
{
    /// <summary>
    ///     A subclass of MeasuredMetric model to be used in batch query settings, where metrics across many resources are mixed together in the response.
    ///     The ResourceId attribute allows grouping/tagging by resource IDs during processing
    /// </summary>
    public class ResourceAssociatedMeasuredMetric : MeasuredMetric
    {
        /// <summary>
        ///     resourceId associated with this metric
        /// </summary>
        public string ResourceId { get; }

        
        public ResourceAssociatedMeasuredMetric(double? value, string resourceId) : base(value)
        {
            ResourceId = resourceId;
        }

        public ResourceAssociatedMeasuredMetric(double? value, List<MeasuredMetricDimension> dimensions, string resourceId) : base(value, dimensions)
        {
            ResourceId = resourceId;
        }
    }
}