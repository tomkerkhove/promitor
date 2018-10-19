using Microsoft.Azure.Management.Monitor.Fluent;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Integrations.AzureMonitor.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promitor.Integrations.AzureMonitor
{
    public class AzureMonitorQueryRunner : IAzureMonitorQueryRunner
    {
        public async Task<IEnumerable<MetricValue>> RunQuery(string metricName, IMetricDefinition metricDefinition, QueryOptions options)
        {
            var azureMonitorQuery = ToAzureMonitorQuery(options.AggregationType, options.MetricFilter, 
                metricDefinition, options.StartTime, options.EndTime, options.MetricGranularity);
            IEnumerable<MetricValue> series = await ExecuteAzureMonitorQuery(metricName, azureMonitorQuery);
            return series;
        }

        public IEnumerable<MetricValue> ExtractResult(IMetricCollection metricCollection, string metricName)
        {
            IMetric metric;
            if (!EnsureOneItemInList(metricCollection.Metrics, out metric))
            {
                throw new MetricNotFoundException(metricName);
            }

            TimeSeriesElement timeSeriesElement;
            if (!EnsureOneItemInList(metric.Timeseries, out timeSeriesElement))
            {
                throw new MetricInformationNotFoundException(metricName, "Unexpected time series format. No times series was found, or more than one time series found.");
            }

            return timeSeriesElement.Data;
        }

        private async Task<IEnumerable<MetricValue>> ExecuteAzureMonitorQuery(string metricName, IWithMetricsQueryExecute monitorQuery)
        {
            IMetricCollection metrics = await monitorQuery.ExecuteAsync();
            IEnumerable<MetricValue> result = ExtractResult(metrics, metricName);
            return result;
        }

        private IWithMetricsQueryExecute ToAzureMonitorQuery(AggregationType metricAggregation, string metricFilter,
            IMetricDefinition metricDefinition, DateTime startTime, DateTime endTime, TimeSpan metricGranularity)
        {
            IWithMetricsQueryExecute metricQuery = metricDefinition.DefineQuery()
                .StartingFrom(startTime)
                .EndsBefore(endTime)
                .WithAggregation(metricAggregation.ToString())
                .WithInterval(metricGranularity);
            
            if (string.IsNullOrWhiteSpace(metricFilter) == false)
            {
                metricQuery.WithOdataFilter(metricFilter);
            }

            return metricQuery;
        }

        private bool EnsureOneItemInList<T>(IReadOnlyList<T> list, out T result)
        {
            result = default(T);
            if (list == null)
            {
                return false;
            }

            int count = list.Count();
            switch (count)
            {
                case 0:
                    return false;
                case 1:
                    result = list.First();
                    return true;
                default:
                    return false;
            }
        }
    }
}
