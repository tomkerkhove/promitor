using System;

namespace Promitor.Scraper.Host.Scraping.Exceptions
{
    public class ScrapingException : Exception
    {
        public string MetricName { get; }

        public ScrapingException(string metricName, string message) : base(message)
        {
            MetricName = metricName;
        }
    }
}