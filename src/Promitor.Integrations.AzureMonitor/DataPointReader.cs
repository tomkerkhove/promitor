using GuardNet;
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
    /// <summary>
    /// Reads generic azure definitions decorated with Capacity or Transactional type
    /// </summary>
    public class DataPointReader
    {
        public const double NO_DATA = 0.0D;

        private readonly IAzureMonitorQueryRunner _runner;

        public DataPointReader(IAzureMonitorQueryRunner runner)
        {
            this._runner = runner;
        }

        public async Task<double> ReadDataPoint(string metricName, AggregationType metricAggregation, string metricFilter, IMetricDefinition metricDefinition, MetricType metricType)
        {
            DateTime utcNow = DateTime.UtcNow;

            QueryOptions options = CreateOptions(metricType, metricAggregation, metricFilter, utcNow);
            IEnumerable<MetricValue> series = await _runner.RunQuery(metricName, metricDefinition, options);

            double dataPoint = ExtractDataPoint(series, options.EndTime, metricAggregation);
            return dataPoint;
        }

        public QueryOptions CreateOptions(MetricType metricType, AggregationType metricAggregation, string metricFilter, DateTime utcNow)
        {
            DateTime endTime = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, utcNow.Second, DateTimeKind.Utc);
            DateTime startTime = GetStartTimeForMetricType(metricType, endTime);
            TimeSpan metricGranularity = GetTimespanForMetricType(metricType);

            var queryOptions = new QueryOptions();
            queryOptions.AggregationType = metricAggregation;
            queryOptions.StartTime = startTime;
            queryOptions.EndTime = endTime;
            queryOptions.MetricFilter = metricFilter;
            queryOptions.MetricGranularity = metricGranularity;

            return queryOptions;
        }

        public double ExtractDataPoint(IEnumerable<MetricValue> series, DateTime endTime, AggregationType metricAggregation)
        {
            MetricValue mostRecentMetricValue = GetMostRecentMetricValue(series, endTime);
            double requestMetricAggregate = InterpretMetricValue(metricAggregation, mostRecentMetricValue);
            return requestMetricAggregate;
        }

        private MetricValue GetMostRecentMetricValue(IEnumerable<MetricValue> timeSeries, DateTime endTime)
        {
            var relevantMetricValue = timeSeries
                .Where(metricValue => metricValue.TimeStamp <= endTime)
                .OrderByDescending(metricValue => metricValue.TimeStamp)
                .FirstOrDefault();

            return relevantMetricValue;
        }

        private double InterpretMetricValue(AggregationType metricAggregation, MetricValue relevantMetricValue)
        {
            switch (metricAggregation)
            {
                case AggregationType.Average:
                    return relevantMetricValue.Average ?? NO_DATA;
                case AggregationType.Count:
                    return relevantMetricValue.Count ?? NO_DATA;
                case AggregationType.Maximum:
                    return relevantMetricValue.Maximum ?? NO_DATA;
                case AggregationType.Minimum:
                    return relevantMetricValue.Minimum ?? NO_DATA;
                case AggregationType.Total:
                    return relevantMetricValue.Total ?? NO_DATA;
                case AggregationType.None:
                    return 0.0D; // TODO: What is the behavior for None aggregation?
                default:
                    throw new Exception($"Unable to determine the metrics value for aggregator '{metricAggregation}'");
            }
        }

        private TimeSpan GetTimespanForMetricType(MetricType metricType)
        {
            switch (metricType)
            {
                // Lowest resolution for capacity metrics is 1 hour
                case MetricType.Capacity:
                    return TimeSpan.FromHours(1);

                // Lowest resolution for transactional metrics is 1 minute
                case MetricType.Transactional:
                    return TimeSpan.FromMinutes(1);

                default:
                    throw new Exception("Do not know how to handle this metric type");
            }
        }

        private static DateTime GetStartTimeForMetricType(MetricType metricType, DateTime endTime)
        {
            DateTime startTime = endTime;
            switch (metricType)
            {
                case MetricType.Capacity:
                    startTime = endTime.AddHours(-2D);
                    break;
                case MetricType.Transactional:
                    startTime = endTime.AddMinutes(-20D);
                    break;
                default:
                    throw new Exception("Do not know how to handle this metric type");
            }

            return startTime;
        }
    }
}
