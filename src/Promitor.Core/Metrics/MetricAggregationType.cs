using System;
using Azure.Monitor.Query.Models;
using Microsoft.Azure.Management.Monitor.Fluent.Models;

namespace Promitor.Core.Metrics;

public enum PromitorMetricAggregationType
{
    None,
    Average,
    Count,
    Minimum,
    Maximum,
    Total
}

public static class MetricAggregationTypeConverter 
{
    public static AggregationType AsLegacyAggregationType(PromitorMetricAggregationType promitorMetricAggregationType) 
    {
        switch(promitorMetricAggregationType) 
        {
            case PromitorMetricAggregationType.None:
                return AggregationType.None;
            case PromitorMetricAggregationType.Average:
                return AggregationType.Average;
            case PromitorMetricAggregationType.Count:
                return AggregationType.Count;
            case PromitorMetricAggregationType.Minimum:
                return AggregationType.Minimum; 
            case PromitorMetricAggregationType.Maximum:
                return AggregationType.Maximum;
            case PromitorMetricAggregationType.Total:
                return AggregationType.Total;
            default:
                throw new ArgumentOutOfRangeException(nameof(promitorMetricAggregationType), $"Cannot convert aggregation type {promitorMetricAggregationType} to legacy model"); 
        }
    } 

    public static MetricAggregationType AsMetricAggregationType(PromitorMetricAggregationType promitorMetricAggregationType) 
    {
        switch(promitorMetricAggregationType) 
        {
            case PromitorMetricAggregationType.None:
                return MetricAggregationType.None;
            case PromitorMetricAggregationType.Average:
                return MetricAggregationType.Average;
            case PromitorMetricAggregationType.Count:
                return MetricAggregationType.Count;
            case PromitorMetricAggregationType.Minimum:
                return MetricAggregationType.Minimum; 
            case PromitorMetricAggregationType.Maximum:
                return MetricAggregationType.Maximum;
            case PromitorMetricAggregationType.Total:
                return MetricAggregationType.Total;
            default:
                throw new ArgumentOutOfRangeException(nameof(promitorMetricAggregationType), $"Cannot convert aggregation type {promitorMetricAggregationType}"); 
        }
    } 
}
