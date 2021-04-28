using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Model
{
    public class MetricDefaults
    {
        public Aggregation Aggregation { get; set; } = new Aggregation();

        public Scraping Scraping { get; set; } = new Scraping();

        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();
    }
}