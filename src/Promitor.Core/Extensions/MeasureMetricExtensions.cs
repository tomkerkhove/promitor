using Promitor.Core.Metrics;

namespace Promitor.Core.Extensions
{
    public static class MeasuredMetricExtensions
    {
        /// <param name="measuredMetric">A time series value</param>
        /// <param name="resourceId">Resource ID to associate the metric with</param>
        /// <returns>Instance of MeasuredMetric subclass with resourceId attached</returns>
        public static ResourceAssociatedMeasuredMetric WithResourceIdAssociation(this MeasuredMetric measuredMetric, string resourceId)
        {
            return measuredMetric.IsDimensional 
                ? new ResourceAssociatedMeasuredMetric(measuredMetric.Value, measuredMetric.Dimensions, resourceId) 
                :  new ResourceAssociatedMeasuredMetric(measuredMetric.Value, resourceId);
        }
    }
}