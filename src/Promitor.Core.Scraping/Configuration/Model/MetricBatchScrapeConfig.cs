namespace Promitor.Core.Scraping.Configuration.Model
{
    public class MetricBatchScrapeConfig
    {
        public bool Enabled { get; set; }
        public int MaxBatchSize { get; set; }
    }
}