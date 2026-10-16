using System;
using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions
{
    public class MetricAggregationValidator
    {
        private readonly MetricDefaults _metricDefaults;

        public MetricAggregationValidator(MetricDefaults metricDefaults)
        {
            _metricDefaults = metricDefaults;
        }

        public IEnumerable<string> Validate(MetricAggregation metricsAggregation)
        {
            if (metricsAggregation?.Interval.HasValue == false
                && _metricDefaults?.Aggregation?.Interval.HasValue == false)
            {
                yield return "No metrics aggregation is configured";
            }

            var aggregationInterval = metricsAggregation?.Interval ?? _metricDefaults?.Aggregation?.Interval;
            foreach (var errorMessage in ValidateOffset(metricsAggregation?.Offset, aggregationInterval))
            {
                yield return errorMessage;
            }
        }

        public static IEnumerable<string> ValidateOffset(TimeSpan? offset, TimeSpan? aggregationInterval)
        {
            if (offset.HasValue == false)
            {
                yield break;
            }

            if (offset.Value < TimeSpan.Zero)
            {
                yield return $"Aggregation offset '{offset.Value:c}' cannot be negative";
            }
            else if (aggregationInterval > TimeSpan.Zero && offset.Value.Ticks % aggregationInterval.Value.Ticks != 0)
            {
                yield return $"Aggregation offset '{offset.Value:c}' has to be a multiple of the aggregation interval '{aggregationInterval.Value:c}'";
            }
        }
    }
}
