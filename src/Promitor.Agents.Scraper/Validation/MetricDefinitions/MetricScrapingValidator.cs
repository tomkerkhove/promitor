using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions
{
    public class MetricScrapingValidator
    {
        private readonly MetricDefaults _metricDefaults;

        public MetricScrapingValidator(MetricDefaults metricDefaults)
        {
            _metricDefaults = metricDefaults;
        }

        public IEnumerable<string> Validate(Scraping metricsScraping)
        {
            if (string.IsNullOrWhiteSpace(metricsScraping?.Schedule)
                && string.IsNullOrWhiteSpace(_metricDefaults?.Scraping?.Schedule))
            {
                yield return "No metrics scraping schedule is configured";
            }
        }
    }
}
