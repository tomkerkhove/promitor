using System;
using System.Collections.Generic;
using System.Linq;

namespace Promitor.Integrations.AzureMonitor
{
    public static class MetricTimeGrainSelector
    {
        /// <summary>
        ///     Determines the point in time to query Azure Monitor up to, shifted back by the configured aggregation offset
        /// </summary>
        /// <param name="now">Current time</param>
        /// <param name="aggregationOffset">How far behind the most recent time grain the value should be read from</param>
        /// <param name="historyWindow">How much history is queried from Azure Monitor</param>
        public static DateTime DetermineQueryEndTime(DateTime now, TimeSpan? aggregationOffset, TimeSpan historyWindow)
        {
            var offset = aggregationOffset ?? TimeSpan.Zero;
            if (offset < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(aggregationOffset), offset, "Aggregation offset cannot be negative");
            }

            if (offset >= historyWindow)
            {
                throw new ArgumentOutOfRangeException(nameof(aggregationOffset), offset, $"Aggregation offset has to be smaller than the queried history window of {historyWindow:c}");
            }

            return now.Subtract(offset);
        }

        /// <summary>
        ///     Selects the most recent time grain that started at or before <paramref name="queryEndTime"/>, being the time grain that contains it
        /// </summary>
        /// <returns>The most recent time grain, or default when none qualifies</returns>
        public static TMetricValue SelectMostRecent<TMetricValue>(IEnumerable<TMetricValue> metricValues, Func<TMetricValue, DateTimeOffset> timeStampSelector, DateTimeOffset queryEndTime)
        {
            return metricValues.Where(metricValue => timeStampSelector(metricValue) <= queryEndTime)
                               .MaxBy(timeStampSelector);
        }
    }
}
