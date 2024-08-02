using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Promitor.Core.Metrics;

namespace Promitor.Integrations.AzureMonitor
{
    public interface IAzureMonitorClient
    {
        public Task<List<MeasuredMetric>> QueryMetricAsync(string metricName, List<string> metricDimensions, PromitorMetricAggregationType aggregationType, TimeSpan aggregationInterval,
            string resourceId, string metricFilter = null, int? metricLimit = null); 

        public Task<List<ResourceAssociatedMeasuredMetric>> BatchQueryMetricAsync(string metricName, List<string> metricDimensions, PromitorMetricAggregationType aggregationType, TimeSpan aggregationInterval,
            List<string >resourceIds, string metricFilter = null, int? metricLimit = null);     
    }
}