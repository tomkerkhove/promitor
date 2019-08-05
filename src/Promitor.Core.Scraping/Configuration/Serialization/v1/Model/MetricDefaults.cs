namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class MetricDefaults
    {
        public Aggregation Aggregation { get; set; } = new Aggregation();

        public Scraping Scraping { get; set; } = new Scraping();
    }
}