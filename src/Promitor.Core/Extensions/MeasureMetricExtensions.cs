using Promitor.Core.Metrics;

namespace Promitor.Core.Extensions
{
    public static class MeasuredMetricExtensions
    {
        /// <summary>
        ///     Get Azure environment information under legacy SDK model
        /// </summary>
        /// <param name="azureCloud">Microsoft Azure cloud</param>
        /// <returns>Azure environment information for specified cloud</returns>
        public static ResourceAssociatedMeasuredMetric WithResourceIdAssociation(this MeasuredMetric measuredMetric, string resourceId)
        {
            return measuredMetric.IsDimensional 
                ? new ResourceAssociatedMeasuredMetric(measuredMetric.Value, measuredMetric.Dimensions, resourceId) 
                :  new ResourceAssociatedMeasuredMetric(measuredMetric.Value, resourceId);
        }
    }
}