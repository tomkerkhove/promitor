using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions
{
    public class MetricAggregationValidator
    {
        private readonly MetricDefaults _metricDefaults;

        public MetricAggregationValidator(MetricDefaults metricDefaults)
        {
            this._metricDefaults = metricDefaults;
        }

        public List<string> Validate(MetricAggregation metricsAggregation)
        {
            var errorMessages = new List<string>();

            if (metricsAggregation?.Interval.HasValue == false
                && _metricDefaults?.Aggregation?.Interval.HasValue == false)
            {
                errorMessages.Add("No metrics aggregation is configured");
            }

            return errorMessages;
        }
    }
}