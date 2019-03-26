using System;

namespace Promitor.Core.Scraping.Configuration.Model
{
    public class MetricDefaults
    {
        public Aggregation Aggregation { get; set; } = new Aggregation();

        public TimeSpan ScrapingInterval { get; set; }
    }
}