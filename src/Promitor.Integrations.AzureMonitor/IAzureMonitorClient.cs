using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Promitor.Core.Metrics;

namespace Promitor.Integrations.AzureMonitor
{
    public interface IAzureMonitorClient
    {
        public Task<List<MeasuredMetric>> QueryMetricAsync(string metricName, List<string> metricDimensions, MetricAggregationType aggregationType, TimeSpan aggregationInterval,
            string resourceId, string metricFilter = null, int? metricLimit = null); 
    }
}