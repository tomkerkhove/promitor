namespace Promitor.Integrations.AzureMonitor.Configuration
{
    public class AzureMonitorMetricBatchScrapeConfig
    {
        public bool Enabled { get; set; } = false;
        public int MaxBatchSize { get; set; }
        public string AzureRegion { get; set; } // Batch scrape endpoints are deployed by region 
    }
}