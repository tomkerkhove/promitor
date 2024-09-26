namespace Promitor.Integrations.AzureMonitor.Configuration
{
    public class AzureMonitorMetricBatchScrapeConfig
    {
        public bool Enabled { get; } = false;
        public int MaxBatchSize { get; }
        public string AzureRegion { get; } // Batch scrape endpoints are deployed by region 
    }
}