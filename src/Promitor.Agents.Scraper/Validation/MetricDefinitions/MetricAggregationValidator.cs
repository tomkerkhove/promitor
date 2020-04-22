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
        }
    }
}