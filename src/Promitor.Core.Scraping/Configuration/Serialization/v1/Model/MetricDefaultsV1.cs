namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class MetricDefaultsV1
    {
        public AggregationV1 Aggregation { get; set; } = new AggregationV1();
        
        public ScrapingV1 Scraping { get; set; } = new ScrapingV1();
    }
}