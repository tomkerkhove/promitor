using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions
{
    public class MetricScrapingValidator
    {
        private readonly MetricDefaults _metricDefaults;

        public MetricScrapingValidator(MetricDefaults metricDefaults)
        {
            _metricDefaults = metricDefaults;
        }

        public List<string> Validate(Scraping metricsScraping)
        {
            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(metricsScraping?.Schedule)
                && string.IsNullOrWhiteSpace(_metricDefaults?.Scraping?.Schedule))
            {
                errorMessages.Add("No metrics scraping schedule is configured");
            }

            return errorMessages;
        }
    }
}
